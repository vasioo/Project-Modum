namespace Modum.Models.BaseModels.Enums
{
    public enum OrderStatus
    {
        NoInformation = 0,
        JustOrdered = 1,
        Processed = 2,
        ApprovedByFactory = 3,
        Travelling = 4,
        ArrivedToLogisticsCompany = 5,
        ArrivedToCustomer = 6,
        Received = 7,
        Returned = 8,
        NotArrived = 9,
    }
}
