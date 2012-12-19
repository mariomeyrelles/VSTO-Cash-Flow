namespace ModernCashFlow.WpfControls
{
    public class ViewModelLocator
    {

        public SummaryViewModel Summary
        {
            get
            {
                return new SummaryViewModel();
            }
        }
    }
}