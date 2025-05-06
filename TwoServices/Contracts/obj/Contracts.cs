namespace TwoServices.Contracts.obj
{
 
        public record CustomerCreated(int Id, string Name);
    public record OrderRequest(int CustomerId);
    public record OrderResponse(string Message);

}
