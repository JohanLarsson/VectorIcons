namespace DigitTrimmer
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using JetBrains.Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private string input;
        private string output;
        private int digits;
        private bool shiftToOrigin;
        private double size;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Input
        {
            get { return this.input; }

            set
            {
                if (value == this.input)
                {
                    return;
                }

                this.input = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Output));
            }
        }

        public string Output => GetOutput();

        public int Digits
        {
            get { return this.digits; }

            set
            {
                if (value == this.digits)
                {
                    return;
                }

                this.digits = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Output));
            }
        }

        public bool ShiftToOrigin
        {
            get { return this.shiftToOrigin; }

            set
            {
                if (value == this.shiftToOrigin)
                {
                    return;
                }

                this.shiftToOrigin = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Output));
            }
        }

        public double Size
        {
            get { return this.size; }

            set
            {
                if (value.Equals(this.size))
                {
                    return;
                }

                this.size = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Output));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string GetOutput()
        {
            if (string.IsNullOrWhiteSpace(this.input))
            {
                return string.Empty;
            }

            try
            {
                return TextConverter.Convert(this.input, this.digits, 1);
                var tokens = Parser.GetTokens(this.input).ToArray();

                return "";
                //var result = geometry.ToString(CultureInfo.InvariantCulture);
                //return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
