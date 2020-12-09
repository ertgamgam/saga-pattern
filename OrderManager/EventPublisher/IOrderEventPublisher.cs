using System.Threading.Tasks;
using OrderManager.Entity;

namespace OrderManager.EventPublisher
{
    public interface IOrderEventPublisher
    {
        Task PublishOrderCreateEvent(Order order);
    }
}