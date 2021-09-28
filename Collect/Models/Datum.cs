using Stylet;
using System;

namespace Collect.Models
{
    class Datum : PropertyChangedBase
    {
        #region Properties
        private double _dateTime;
        public double DateTime
        {
            get { return _dateTime; }
            set { this.SetAndNotify(ref this._dateTime, value); }
        }
        private string _tagName;
        public string TagName
        {
            get { return _tagName; }
            set { this.SetAndNotify(ref this._tagName, value); }
        }
        private double _value;
        public double Value
        {
            get { return _value; }
            set { this.SetAndNotify(ref this._value, value); }
        }
        #endregion
    }
}
