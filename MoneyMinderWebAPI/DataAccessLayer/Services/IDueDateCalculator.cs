
namespace MoneyMinderWebAPI.DataAccessLayer.Services
{
    public interface IDueDateCalculator
    {
        DateTime CalculateDueDate(DateTime priorDueDate, int paymentFrequency);
    }
}
