using System;
using System.Reactive.Linq;
using ReactiveUI;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _content = null!;

        public MainWindowViewModel(Database db)
        {
            Content = List = new TodoListViewModel(db.GetItems());
        }

        public ViewModelBase Content
        {
            get => _content;
            private set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        private TodoListViewModel List { get; }

        public void AddItem()
        {
            var vm = new AddItemViewModel();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            vm.Ok.Merge(vm.Cancel.Select(_ => (TodoItem)null))
                .Take(1)
                .Subscribe(model =>
                {
                    if (model != null)
                    {
                        List.Items.Add(model);
                    }

                    Content = List;
                });
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            Content = vm;
        }
    }
}