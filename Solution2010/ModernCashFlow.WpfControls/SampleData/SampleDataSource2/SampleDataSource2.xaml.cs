﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.
namespace Expression.Blend.SampleData.SampleDataSource2
{
	using System; 

// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class SampleDataSource2 { }
#else

	public class SampleDataSource2 : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public SampleDataSource2()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/ModernCashFlow.WpfControls;component/SampleData/SampleDataSource2/SampleDataSource2.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					System.Windows.Application.LoadComponent(this, resourceUri);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private string _StatusName = string.Empty;

		public string StatusName
		{
			get
			{
				return this._StatusName;
			}

			set
			{
				if (this._StatusName != value)
				{
					this._StatusName = value;
					this.OnPropertyChanged("StatusName");
				}
			}
		}
	}
#endif
}
