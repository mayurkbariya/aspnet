using System.Linq.Expressions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.AppTransactions.Models;

public class AppTransactionAdminDto : AppTransactionDto
{
    public string UserId { get; set; }
    public string User { get; set; }
}
public class AppTransactionDto
{
    public string StripeSubscriptionId { get; set; }
    public string StripePaymentId { get; set; }
    public double Amount { get; set; }
    public double Fee { get; set; }
    public string Url { get; set; }
    public string Status { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class AppTransactionSelector
{
    public static Expression<Func<AppTransaction, AppTransactionDto>> Selector = p => new AppTransactionDto()
    {
        Amount = p.Amount,
        Fee = p.Fee,
        Status = p.Status,
        Url = p.Url,
        FromDate = p.FromDate,
        InvoiceDate = p.InvoiceDate,
        ToDate = p.ToDate,
        StripePaymentId = p.StripePaymentId,
        StripeSubscriptionId = p.StripeSubscriptionId
    };
    public static Expression<Func<AppTransaction, AppTransactionDto>> SelectorAdmin = p => new AppTransactionAdminDto()
    {
        Amount = p.Amount,
        Fee = p.Fee,
        Status = p.Status,
        Url = p.Url,
        FromDate = p.FromDate,
        InvoiceDate = p.InvoiceDate,
        ToDate = p.ToDate,
        StripePaymentId = p.StripePaymentId,
        StripeSubscriptionId = p.StripeSubscriptionId,
        User = p.User.FullName,
        UserId = p.UserId
    };

}