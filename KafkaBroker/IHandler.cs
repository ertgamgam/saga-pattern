using System.Threading.Tasks;

namespace KafkaBroker
{
    public interface IHandler<in TMessage>
    {
        Task Handle(TMessage message);
    }
}