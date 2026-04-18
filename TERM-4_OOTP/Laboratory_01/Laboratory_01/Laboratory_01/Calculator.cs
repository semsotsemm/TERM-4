using System;

namespace Laboratory_01
{
    public delegate void ConversionHandler(double result);

    public class Calculator
    {
        public event ConversionHandler OnCalculated;

        public void Convert(double value, string fromUnit, string toUnit)
        {
            double result = 0;

            double baseValue = ToBase(value, fromUnit);

            result = FromBase(baseValue, toUnit);

            OnCalculated?.Invoke(result);
        }

        private double ToBase(double val, string unit)
        {
            switch (unit)
            {
                // Длина
                case "Метры": return val;
                case "Сантиметры": return val * 0.01;
                case "Футы": return val * 0.3048;
                case "Дюймы": return val * 0.0254;

                // Вес
                case "Килограммы": return val;
                case "Граммы": return val * 0.001;
                case "Фунты": return val * 0.453592;
                case "Унции": return val * 0.0283495;

                // Объем 
                case "Литры": return val;
                case "Миллилитры": return val * 0.001;
                case "Галлоны": return val * 3.78541;

                default: return 0; 
            }
        }

        private double FromBase(double baseVal, string unit)
        {
            switch (unit)
            {
                // Длина
                case "Метры": return baseVal;
                case "Сантиметры": return baseVal / 0.01;
                case "Футы": return baseVal / 0.3048;
                case "Дюймы": return baseVal / 0.0254;

                // Вес
                case "Килограммы": return baseVal;
                case "Граммы": return baseVal / 0.001;
                case "Фунты": return baseVal / 0.453592;
                case "Унции": return baseVal / 0.0283495;

                // Объем
                case "Литры": return baseVal;
                case "Миллилитры": return baseVal / 0.001;
                case "Галлоны": return baseVal / 3.78541;

                default: return 0;
            }
        }
    }
}