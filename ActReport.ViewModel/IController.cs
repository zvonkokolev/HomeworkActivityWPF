using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ActReport.ViewModel
{
	public interface IController
	{
		public void ShowWindow(BaseViewModel viewModel)
		{

		}

		public void CloseWindow(Window window)
		{

		}
	}
}
