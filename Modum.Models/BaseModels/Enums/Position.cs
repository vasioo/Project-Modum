namespace Modum.Models.BaseModels.Enums
{
    public enum Position
    {
        SuperAdmin, //can access every single thing in the website including the development part
        CEO,//can access every single thing in the website apart from the development part
        CTO,//can access every single thing in the website apart from the development part(negotiable)
        CFO,//can access every single thing in the website apart from the development part
        Admin,//can access everything in the website as long as he/she has rights
        Manager,//can access limited things in the website mainly statistics
        Seller,//can access payment information and ad api's
        Cashier,//can access daily payment and payment operations
        Customer,//can only access front end besides from admin panels
    }
}
