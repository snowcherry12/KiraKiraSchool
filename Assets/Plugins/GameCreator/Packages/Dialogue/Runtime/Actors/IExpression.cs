using System.Threading.Tasks;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Actor Type")]
    public interface IExpression
    {
        Task OnStart(Args args);
        Task OnEnd(Args args);
    }
}