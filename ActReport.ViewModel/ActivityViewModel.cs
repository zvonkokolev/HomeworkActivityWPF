using ActReport.Core.Contracts;
using ActReport.Core.Entities;
using ActReport.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ActReport.ViewModel
{
	public class ActivityViewModel : BaseViewModel
	{

		private DateTime _date;
		private DateTime _startTime;
		private DateTime _endTime;
		private string _activityText;
		private Employee _employee;
		private ObservableCollection<Activity> _activities;
		private Activity _selectedActivity;

		public DateTime Date
		{
			get => _date;
			set
			{
				_date = value;
				OnPropertyChanged(nameof(Date));
			}
		}

		public DateTime StartTime
		{
			get => _startTime;
			set
			{
				_startTime = value;
				OnPropertyChanged(nameof(StartTime));
			}
		}

		public DateTime EndTime
		{
			get => _endTime;
			set
			{
				_endTime = value;
				OnPropertyChanged(nameof(EndTime));
			}
		}

		public string ActivityText
		{
			get => _activityText;
			set
			{
				_activityText = value;
				OnPropertyChanged(nameof(ActivityText));
			}
		}

		public ObservableCollection<Activity> Activities
		{
			get => _activities;
			set
			{
				_activities = value;
				OnPropertyChanged(nameof(Activities));
			}
		}

		public Activity SelectedActivity
		{
			get => _selectedActivity;
			set
			{
				_selectedActivity = value;
				OnPropertyChanged(nameof(SelectedActivity));
			}
		}

		public string FullName => $"{_employee.FirstName} {_employee.LastName}";

		public ActivityViewModel(IController controller, Employee employee) : base(controller)
		{
			_employee = employee;
			using IUnitOfWork uow = new UnitOfWork();
			Activities = new ObservableCollection<Activity>(
				 uow.ActivityRepository
				 .Get(filter: x => x.Employee_Id == employee.Id, orderBy: coll => coll.OrderBy(activity => activity.Date)
				 .ThenBy(activity => activity.StartTime)));
			Activities.CollectionChanged += Activities_CollectionChanged;
		}

		private void Activities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				using (IUnitOfWork uow = new UnitOfWork())
				{
					foreach (var item in e.OldItems)
					{
						uow.ActivityRepository.Delete((item as Activity).Id);
					}
					uow.Save();
				}
			}
		}

		// Commands
		private ICommand _cmdNewActivity;
		public ICommand CmdNewActivity
		{
			get
			{
				Activity activity = new Activity();

				if (_cmdNewActivity == null)
				{
					_cmdNewActivity = new RelayCommand(
					  execute: _ =>
					  {
						  using IUnitOfWork uow = new UnitOfWork();

						  activity.Date = Date;
						  activity.StartTime = StartTime;
						  activity.EndTime = EndTime;
						  activity.ActivityText = ActivityText;
						  
						  uow.ActivityRepository.Insert(activity);
						  uow.Save();
					  },
					  canExecute: _ => activity != null);
				}
				return _cmdNewActivity;
			}
			set { _cmdNewActivity = value; }
		}

		private ICommand _cmdEditActivity;
		public ICommand CmdEditActivity
		{
			get
			{
				if (_cmdNewActivity == null)
				{
					_cmdNewActivity = new RelayCommand(
					  execute: _ =>
					  {
						  using IUnitOfWork uow = new UnitOfWork();

						  _selectedActivity.Date = Date;
						  _selectedActivity.StartTime = StartTime;
						  _selectedActivity.EndTime = EndTime;
						  _selectedActivity.ActivityText = ActivityText;

						  uow.ActivityRepository.Update(_selectedActivity);
						  uow.Save();
					  },
					  canExecute: _ => _selectedActivity != null);
				}
				return _cmdEditActivity;
			}
			set { _cmdEditActivity = value; }
		}

		private ICommand _cmdDeleteActivity;
		public ICommand CmdDeleteActivity
		{
			get
			{
				if (_cmdNewActivity == null)
				{
					_cmdNewActivity = new RelayCommand(
					  execute: _ =>
					  {
						  using IUnitOfWork uow = new UnitOfWork();

						  //_selectedActivity.Date = Date;
						  //_selectedActivity.StartTime = StartTime;
						  //_selectedActivity.EndTime = EndTime;
						  //_selectedActivity.ActivityText = ActivityText;

						  uow.ActivityRepository.Delete(_selectedActivity);
						  uow.Save();
					  },
					  canExecute: _ => _selectedActivity != null);
				}
				return _cmdDeleteActivity;
			}
			set { _cmdDeleteActivity = value; }
		}

	}
}
