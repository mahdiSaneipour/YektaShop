namespace BN_Project.Core.Response.DataResponse
{
    public class DataResponse<T> where T : class
    {
        public string? Message { get; set; }

        public Response.Status.Status Status { get; set; }

        public T Data { get; set; }
    }
}
