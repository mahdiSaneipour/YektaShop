using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.Entities;
using BN_Project.Domain.Enum.Order;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.UserProfile.Order;

namespace BN_Project.Core.Services.Implementations
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductServices _productServices;

        public OrderServices(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository
            , IProductServices productServices)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productServices = productServices;
        }

        public async Task<BaseResponse> AddProductToBasket(int colorId, int userId)
        {
            BaseResponse result = new BaseResponse();

            var order = await _orderRepository.GetSingle(o => o.UserId == userId 
            && o.Status == OrderStatus.AwaitingPayment);

            var color = await _productServices.GetColorByColorId(colorId);

            var product = await _productServices.GetProductWithIncludesByColorId(colorId);

            if(product.Status != Status.Success)
            {
                result.Status = Status.NotValid;
                result.Message = "رنگ کاربر معتبر نمیباشد";

                return result;
            }

            if (color.Count >= 1)
            {
                result.Status = Status.DontHave;
                result.Message = "این تعداد رنگ موجود نیست";

                return result;
            }

            long price = color.Price;
            long finalPrice = color.Price;

            if (await _productServices.AnyDiscount(colorId))
            {
                /*finalPrice = Tools.Tools.PercentagePrice(finalPrice,
                    _productServices.GetDi);*/
            }

            if (order == null)
            {
                Order newOrder = new Order()
                {
                    UserId = userId,
                    Status = OrderStatus.AwaitingPayment,
                    FinalPrice = finalPrice,
                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                            ColorId = colorId,
                            Price = price,
                            Count = 1,
                            FinalPrice = finalPrice,
                        }
                    }
                };

                await _orderRepository.Insert(newOrder);
            } else
            {
                if(await _orderDetailRepository.IsThereAny(od => od.ColorId == colorId 
                && od.OrderId == order.Id))
                {
                    var orderDetail = await _orderDetailRepository.GetSingle(od => od.ColorId == colorId
                        && od.OrderId == order.Id);

                    orderDetail.Count++;

                    _orderDetailRepository.Update(orderDetail);
                } else
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderId = order.Id,
                        Price = price,
                        FinalPrice = finalPrice,
                        ColorId = colorId,
                        Count = 1
                    };

                    await _orderDetailRepository.Insert(orderDetail);
                }
            }

            await _orderRepository.SaveChanges();

            int orderId = order.OrderDetails.FirstOrDefault(od => od.ColorId == colorId).Id;

            await SetPricesInOrderDetail(orderId);

            result.Status = Status.Success;
            result.Message = "محصول با موفقیت به سبد خرید افزوده شد";

            return result;
        }

        public async Task<BaseResponse> ChangeProductOrderCount(int orderDetailId, bool status)
        {
            BaseResponse result = new BaseResponse();

            var orderDetail = await _orderDetailRepository.GetSingle(od => od.Id == orderDetailId);

            if (orderDetail == null)
            {
                result.Status = Status.NotFound;
                result.Message = "سفارشی با این ایدی پیدا نشد";

                return result;
            }

            if (status)
            {
                orderDetail.Count++;
            } else
            {
                orderDetail.Count--;
            }

            await SetPricesInOrderDetail(orderDetailId);

            _orderDetailRepository.Update(orderDetail);
            await _orderDetailRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "مقدار سفارش با موفقیت تغییر کرد";

            return result;
        }

        public async Task<BaseResponse> DeleteOrderDetailByOrderDetailId(int orderDetailId)
        {
            BaseResponse result = new BaseResponse();

            var orderDetail = await _orderDetailRepository.GetSingle(od => od.Id == orderDetailId);

            if (orderDetail == null)
            {
                result.Status = Status.NotFound;
                result.Message = "سفارشی با این ایدی پیدا نشد";

                return result;
            }

            _orderDetailRepository.Delete(orderDetail);
            await _orderDetailRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "سفارش با موفقیت حذف شد";

            return result;
        }

        public async Task<DataResponse<List<BoxBasketListViewModel>>> GetBasketOrders(int userId)
        {
            DataResponse<List<BoxBasketListViewModel>> result = new DataResponse<List<BoxBasketListViewModel>>();
            List<BoxBasketListViewModel> data = new List<BoxBasketListViewModel>();

            var basket = await _orderRepository.GetBasketOrderWithIncludeOrderDetailsAndProductAndDiscountAndColorByUserId(userId);

            if (basket == null)
            {
                result.Status = Status.NotFound;
                result.Message = "سبد خریدخالی میباشد";
                result.Data = data;
            } else
            {
                foreach(var order in basket.OrderDetails)
                {
                    long discount = 0;

                    /*if (order.Color.Product.Discounts.Count != 0)
                    {
                        discount = Tools.Tools.DiscountPrice(order.Color.Price,
                            order.Color.Product.Discounts.OrderBy(d => d.Percent)
                            .FirstOrDefault().Percent) * order.Count;
                    }*/

                    data.Add(new BoxBasketListViewModel()
                    {
                        OrderDetailId = order.Id,
                        ProductId = order.Color.ProductId,
                        Image = order.Color.Product.Image,
                        Name = order.Color.Product.Name,
                        ColorName = order.Color.Name,
                        Price = order.FinalPrice,
                        ColorId = order.ColorId,
                        Hex = order.Color.Hex,
                        Count = order.Count,
                        Discount = discount
                    }); 
                }

                result.Status = Status.Success;
                result.Message = "موارد سبد خرید پیدا شد";
                result.Data = data;
            }

            return result;
        }

        public async Task<DataResponse<List<BoxOrderListViewModel>>> GetBoxOrderList(OrderStatus orderStatus, int userId)
        {
            DataResponse<List<BoxOrderListViewModel>> result = new DataResponse<List<BoxOrderListViewModel>>();

            var orders = await _orderRepository.GetOrderBoxByStatusWithIncludeOrderDetail(orderStatus, userId);

            if (orders.Count == 0)
            {
                result.Data = new List<BoxOrderListViewModel>();
                result.Status = Response.Status.Status.NotFound;
                result.Message = "سفارشی پیدا نشد";
            }
            else
            {
                List<BoxOrderListViewModel> data = new List<BoxOrderListViewModel>();
                foreach (var order in orders)
                {
                    data.Add(new BoxOrderListViewModel()
                    {
                        CreateDate = order.Create,
                        FinalPrice = order.FinalPrice,
                        OrderId = order.Id,
                        Status = OrderStatus.AwaitingPayment,
                        ProductImages = await _orderRepository.GetProductImagesByOrderId(order.Id)
                    });
                }

                result.Data = data;
                result.Status = Response.Status.Status.Success;
                result.Message = "سفارش ها پیدا شدند";
            }


            return result;
        }

        public async Task<DataResponse<FactorCompViewModel>> GetFactorCompModel(int userId)
        {
            DataResponse<FactorCompViewModel> result = new DataResponse<FactorCompViewModel>();

            if(userId == 0)
            {
                result.Status = Status.NotFound;
                result.Message = "کاربری پیدا نشد";

                return result;
            }

            var order = await _orderRepository.GetBasketOrderWithIncludeOrderDetailsAndProductAndDiscountAndColorByUserId(userId);

            FactorCompViewModel data = new FactorCompViewModel();

            data.Price = (int) order.FinalPrice;
            data.TotalPrice = 0;
            data.Discount = 0;

            result.Status = Status.Success;
            result.Message = "مقادیر با موفقیت پیدا شدند";
            result.Data = data;

            return result;
        }

        public async Task<BaseResponse> SetPricesInOrderDetail(int orderDetailId)
        {
            BaseResponse result = new BaseResponse();

            var orderDetail = await _orderDetailRepository.GetSingle(od => od.Id == orderDetailId);

            if (orderDetail == null)
            {
                result.Status = Status.NotFound;
                result.Message = "هیچ سفارشی ب این آیدی پیدا نشد";

                return result;
            }

            long price = await _productServices.GetPriceByColorId(orderDetail.ColorId);
            long finalPrice = await _productServices.GetPriceWithDiscountByColorId(orderDetail.ColorId);

            orderDetail.FinalPrice = finalPrice * orderDetail.Count;
            orderDetail.Price = price * orderDetail.Count;

            _orderDetailRepository.Update(orderDetail);
            await _orderDetailRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "قیمت محصول با موفقیت آپدیت شد";

            return result;
        }
    }
}