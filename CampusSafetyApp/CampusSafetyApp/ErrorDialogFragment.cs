using Android.App;
using Android.OS;

namespace CampusSafetyApp
{
    internal class ErrorDialogFragment : DialogFragment
    {

        public new Dialog Dialog { get; private set; }

        public ErrorDialogFragment(Dialog dialog)
        {
            Dialog = dialog;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return Dialog;
        }
    }
}
