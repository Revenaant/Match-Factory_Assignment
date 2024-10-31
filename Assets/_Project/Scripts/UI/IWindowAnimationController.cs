using System.Collections;

namespace Revenaant.Project.UI
{
    public interface IWindowAnimationController
    {
        bool CanShow { get; }

        IEnumerator ShowEnumerator();

        IEnumerator HideEnumerator();

        void HideInstantly();
    }
}