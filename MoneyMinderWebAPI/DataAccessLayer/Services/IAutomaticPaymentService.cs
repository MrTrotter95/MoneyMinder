namespace MoneyMinderWebAPI.DataAccessLayer.Services
{
    public interface IAutomaticPaymentService
    {
        List<ExpenseToPay> GenerateExpensesUntilNextPay(int accountId, DateTime currentDate);

        List<ExpenseToPay> GetDueAutomaticPayments(int accountID);
    }
}
