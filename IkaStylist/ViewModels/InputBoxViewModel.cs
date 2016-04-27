using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using IkaStylist.Models;

namespace IkaStylist.ViewModels
{
    public class InputBoxViewModel : ViewModel
    {
        public class Parameter
        {
            public Parameter(string prompt, string title = "", string DefaultResponse = "")
            {
                this.Prompt = prompt;
                this.Title = title;
                this.Response = DefaultResponse;
                this.Result = false;
            }

            /// <summary> 自身のコピーを生成します </summary>
            public object Clone()
            {
                return new Parameter(this.Response, this.Prompt, this.Title);
            }

            public string Response;
            public string Prompt;
            public string Title;
            public bool Result;
        }

        public InputBoxViewModel(InputBoxViewModel.Parameter param)
        {
            this.Param = param;
            this.Prompt = param.Prompt;
            this.Response = param.Response;
        }

        #region Param変更通知プロパティ
        private Parameter _Origin;
        private Parameter _Param;

        public Parameter Param
        {
            get
            { return _Param; }
            set
            {
                if (_Origin == value) return;
                _Origin = value;
                _Param = (Parameter)_Origin.Clone();
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Prompt変更通知プロパティ
        private string _Prompt;

        public string Prompt
        {
            get
            { return _Prompt; }
            set
            {
                if (_Prompt == value)
                    return;
                _Prompt = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Response変更通知プロパティ
        private string _Response;

        public string Response
        {
            get
            { return _Response; }
            set
            {
                if (_Response == value)
                    return;
                _Response = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region OkCommand
        private ViewModelCommand _OkCommand;

        public ViewModelCommand OkCommand
        {
            get
            {
                if (_OkCommand == null)
                {
                    _OkCommand = new ViewModelCommand(Ok);
                }
                return _OkCommand;
            }
        }

        public void Ok()
        {
            _Origin.Response = Response;
            _Origin.Result = true;
            Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
        }
        #endregion

    }
}
