using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.InventarioCommands
{
    public class LimpiarButtonCommand : CommandBase
    {
        private InventarioViewModel _ViewModel;
        public LimpiarButtonCommand(InventarioViewModel viewModel)
        {
            _ViewModel = viewModel;
            _ViewModel.PropertyChanged += _ViewModel_PropertyChanged;
        }

        private void _ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InventarioViewModel.SelectedBodega))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return (_ViewModel.SelectedBodega != null);
        }

        public override void Execute(object? parameter)
        {
            _ViewModel.SelectedBodega = null;
        }
    }
}
