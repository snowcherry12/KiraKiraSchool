using System;
using System.Threading.Tasks;

namespace GameCreator.Runtime.Characters
{
    public interface INavMeshTraverseLink
    {
        Task Traverse(Character character, Action onTraverseComplete);
    }
}