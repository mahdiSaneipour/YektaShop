using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.Entities;
using BN_Project.Domain.Enum.Order;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.Admin;
using BN_Project.Domain.ViewModel.UserProfile.Order;
using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace BN_Project.Core.Services.Implementations
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductServices _productServices;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrderServices(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository
            , IProductServices productServices, IHttpContextAccessor contextAccessor)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productServices = productServices;
            _contextAccessor = contextAccessor;
        }

        public async Task<BaseResponse> AddProductToBasket(int colorId)
        {
            BaseResponse result = new BaseResponse();

            int userId = Int32.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault().Value);

            var order = await _orderRepository.GetSingle(o => o.UserId == userId
            && o.Status == OrderStatus.AwaitingPayment);

            var color = await _productServices.GetColorByColorId(colorId);

            if (color.Count < 1)
            {
                result.Status = Status.NotValid;
                result.Message = "رنگ کاربر معتبر نمیباشد";

                return result;
            }

            if (color.Count < 1)
            {
                result.Status = Status.DontHave;
                result.Message = "این تعداد رنگ موجود نیست";

                return result;
            }

            if (order == null)
            {
                Order newOrder = new Order()
                {
                    UserId = userId,
                    Status = OrderStatus.AwaitingPayment,
                    FinalPrice = color.Price,
                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                            ColorId = colorId,
                            Price = color.Price,
                            Count = 1,
                            FinalPrice = color.Price,
                        }
                    }
                };

                await _orderRepository.Insert(newOrder);

                order = newOrder;
            }
            else
            {
                if (await _orderDetailRepository.IsThereAny(od => od.ColorId == colorId
                && od.OrderId == order.Id))
                {
                    var orderDetail = await _orderDetailRepository.GetSingle(od => od.ColorId == colorId
                        && od.OrderId == order.Id);

                    orderDetail.Count++;

                    _orderDetailRepository.Update(orderDetail);
                }
                else
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderId = order.Id,
                        Price = color.Price,
                        FinalPrice = color.Price,
                        ColorId = colorId,
                        Count = 1
                    };

                    await _orderDetailRepository.Insert(orderDetail);
                }
            }

            await _orderRepository.SaveChanges();

            await _productServices.ChangeColorCount(colorId, -1);

            await SetPriceInOrder(order.Id);

            result.Status = Status.Success;
            result.Message = "محصول با موفقیت به سبد خرید افزوده شد";

            return result;
        }

        public async Task<BaseResponse> ChangeProductOrderCount(int orderDetailId, int count = 1)
        {
            BaseResponse result = new BaseResponse();

            var orderDetail = await _orderDetailRepository.GetSingle(od => od.Id == orderDetailId);

            if (orderDetail == null)
            {
                result.Status = Status.NotFound;
                result.Message = "سفارشی با این ایدی پیدا نشد";

                return result;
            }

            if (count == 0 && orderDetail.Count <= count)
            {
                result.Status = Status.DontHave;
                result.Message = "این تعداد حصول موجود نیست";

                return result;
            }


            orderDetail.Count += count;


            await _productServices.ChangeColorCount(orderDetail.ColorId, count);


            await SetPriceInOrder(orderDetail.OrderId);

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

            await _productServices.ChangeColorCount(orderDetail.ColorId, orderDetail.Count);

            _orderDetailRepository.Delete(orderDetail);
            await _orderDetailRepository.SaveChanges();

            await SetPriceInOrder(orderDetail.OrderId);

            result.Status = Status.Success;
            result.Message = "سفارش با موفقیت حذف شد";

            return result;
        }

        public async Task<DataResponse<List<BoxBasketListViewModel>>> GetBasketOrders(int userId)
        {
            DataResponse<List<BoxBasketListViewModel>> result = new DataResponse<List<BoxBasketListViewModel>>();
            List<BoxBasketListViewModel> data = new List<BoxBasketListViewModel>();

            int basketId = await _orderRepository.GetBasketIdByUserId(userId);

            await SetPriceInOrder(basketId);

            var basket = await _orderRepository.GetBasketByIdByIncludes(basketId);

            if (basket == null)
            {
                result.Status = Status.NotFound;
                result.Message = "سبد خریدخالی میباشد";
                result.Data = data;
            }
            else
            {
                foreach (var order in basket.OrderDetails)
                {
                    int discount = 0;
                    int finalPrice = 0;

                    finalPrice = order.FinalPrice;
                    discount = order.Price - order.FinalPrice;

                    /*if (await _productServices.AnyDiscountByColorId(order.ColorId))
                    {
                        discount = Tools.Tools.DiscountPrice(order.Color.Price,
                            await _productServices.GetDiscountPercentByColorId(order.ColorId));

                        finalPrice = (order.Color.Price - discount) * order.Count;
                    } else
                    {
                        finalPrice = order.Color.Price * order.Count;
                    }*/

                    data.Add(new BoxBasketListViewModel()
                    {
                        OrderDetailId = order.Id,
                        ProductId = order.Color.ProductId,
                        Image = order.Color.Product.Image,
                        Name = order.Color.Product.Name,
                        ColorName = order.Color.Name,
                        Price = finalPrice,
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

            if (userId == 0)
            {
                result.Status = Status.NotFound;
                result.Message = "کاربری پیدا نشد";

                return result;
            }

            var order = await _orderRepository.GetBasketOrderWithIncludeOrderDetailsAndProductAndDiscountAndColorByUserId(userId);

            FactorCompViewModel data = new FactorCompViewModel();

            int count = 0;

            foreach (var orderDetail in order.OrderDetails)
            {
                count += orderDetail.Count;
            }

            data.Count = count;
            data.Price = (int)order.FinalPrice;
            data.TotalPrice = order.TotalPrice;
            data.Discount = data.TotalPrice - data.Price;
            data.DiscountCode = ((order.Discount != null) ? order.Discount.Code : "");

            result.Status = Status.Success;
            result.Message = "مقادیر با موفقیت پیدا شدند";
            result.Data = data;

            return result;
        }

        public async Task<int> GetFinalPriceForBasket(int userId)
        {
            var item = await _orderRepository.GetSingle(n => n.UserId == userId && n.Status == 0);
            return (int)item.FinalPrice;
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

            int discount = 0;

            var order = await _orderDetailRepository.GetOrderByOrderDetail(orderDetailId);

            var publicDiscount = await _productServices.GetDiscountPercentByColorId(orderDetail.ColorId);

            List<int> discounts = new List<int>();

            if (order.DiscountId != null)
            {
                foreach (var dp in order.Discount.DiscountProduct)
                {
                    if (dp.Product.Colors.Any(c => c.Id == orderDetail.ColorId))
                    {
                        discounts.Add(dp.Discount.Percent);
                    }
                }


            }

            discounts.Add(publicDiscount);

            discount = discounts.Max();

            int price = await _productServices.GetPriceByColorId(orderDetail.ColorId);

            orderDetail.FinalPrice = Tools.Tools.PercentagePrice(price, discount) * orderDetail.Count;
            orderDetail.Price = price * orderDetail.Count;

            _orderDetailRepository.Update(orderDetail);
            await _orderDetailRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "قیمت محصول با موفقیت آپدیت شد";

            return result;
        }

        public async Task<BaseResponse> SetPriceInOrder(int orderId)
        {
            BaseResponse result = new BaseResponse();

            Order order = await _orderRepository.GetOrderWithIncludeOrderDetail(orderId);

            if (order == null)
            {
                result.Status = Status.NotFound;
                result.Message = "همچین فاکتوری پیدا نشد";

                return result;
            }

            foreach (var orderDetail in order.OrderDetails)
            {
                await SetPricesInOrderDetail(orderDetail.Id);
            }

            int finalPrice = 0;
            int totalPrice = 0;

            foreach (var orderDetail in order.OrderDetails)
            {
                finalPrice += orderDetail.FinalPrice;
                totalPrice += orderDetail.Price;
            }

            order.FinalPrice = finalPrice;
            order.TotalPrice = totalPrice;

            _orderRepository.Update(order);
            await _orderRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "فاکتور پیدا شد و آپدیت شد";

            return result;
        }

        public async Task<BaseResponse> ApplyDiscount(string discount)
        {
            BaseResponse result = new BaseResponse();

            var discountE = await _productServices.GetDiscountByDiscountCodeWithIncludeProducts(discount);

            if (discountE == null)
            {
                result.Status = Status.NotValid;
                result.Message = "کد تخفیف معتبر نمیباشد";

                return result;
            }

            int userId = Int32.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault().Value);
            var order = await _orderRepository.GetBasketOrderWithIncludeOrderDetailsAndProductAndDiscountAndColorByUserId(userId);


            if (discountE.DiscountProduct == null)
            {
                order.FinalPrice = Tools.Tools.PercentagePrice((int)order.FinalPrice, discountE.Percent);
            }
            else
            {
                foreach (var product in discountE.DiscountProduct.Select(dp => dp.Product))
                {
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        if (product.Colors.Any(c => c.Id == orderDetail.ColorId))
                        {
                            int defaultDicount = (int)Tools.Tools.HowManyPercent(orderDetail.Price, orderDetail.FinalPrice);

                            if (discountE.Percent > defaultDicount)
                            {
                                orderDetail.FinalPrice = Tools.Tools.PercentagePrice(orderDetail.Price, discountE.Percent);

                                _orderDetailRepository.Update(orderDetail);

                                await SetPriceInOrder(orderDetail.OrderId);
                            }

                        }
                    }
                }
            }

            order.DiscountId = discountE.Id;
            _orderRepository.Update(order);

            await _orderRepository.SaveChanges();

            return result;
        }

        public async Task CheckOrderDetailsInBasket()
        {
            int userId = Int32.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault().Value);

            int basketId = await _orderRepository.GetBasketIdByUserId(userId);

            await SetPriceInOrder(basketId);

            var basket = await _orderRepository.GetBasketByIdByIncludes(basketId);

            foreach (var orderDetail in basket.OrderDetails)
            {
                if (orderDetail.ExpireTime >= DateTime.Now)
                {
                    _orderDetailRepository.Delete(orderDetail);
                    await _orderDetailRepository.SaveChanges();
                }
            }
        }

        public async Task<DataResponse<List<ChartDataViewModel>>> GetChartDataForMostSellsInPast10Days()
        {
            DataResponse<List<ChartDataViewModel>> result = new DataResponse<List<ChartDataViewModel>>();

            var data = await _orderRepository.GetChartDataForMostSellsInPast10Days();

            result.Data = data;
            result.Status = Status.Success;
            result.Message = "آمار با موفقیت استخراج شد";

            return result;
        }

        public async Task<DataResponse<List<ChartDataViewModel>>> GetChartDataForMostSellsThisMonth()
        {
            DataResponse<List<ChartDataViewModel>> result = new DataResponse<List<ChartDataViewModel>>();

            var data = await _orderRepository.GetChartDataForMostSellsThisMonth();

            result.Data = data;
            result.Status = Status.Success;
            result.Message = "آمار با موفقیت استخراج شد";

            return result;
        }

        public async Task<DataResponse<List<ChartDataViewModel>>> GetChartDataForMost5PopularProduct()
        {
            DataResponse<List<ChartDataViewModel>> result = new DataResponse<List<ChartDataViewModel>>();

            var data = await _orderRepository.GetChartDataForMost5PopularProduct();

            result.Data = data;
            result.Status = Status.Success;
            result.Message = "آمار با موفقیت استخراج شد";

            return result;
        }
    }
}