using BikeRental.Domain.Exceptions;
using BuildingBlocks.Common;

namespace BikeRental.Domain.ValueObjects
{
    public class CNPJ : ValueObject
    {
        public string Value { get; private set; }
        protected CNPJ() { }
        protected CNPJ(string cnpj)
        {
            Value = cnpj;
        }

        public static CNPJ Parse(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
                throw new DomainException($"{cnpj} is not a valid {nameof(cnpj)}");

            int[] multiplier1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplier2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int sum;
            int rest;
            string digit;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                throw new DomainException($"{cnpj} is not a valid {nameof(cnpj)}");

            tempCnpj = cnpj.Substring(0, 12);
            sum = 0;
            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];
            rest = (sum % 11);
            rest = rest < 2 ? 0 : 11 - rest;
            digit = rest.ToString();
            tempCnpj = tempCnpj + digit;
            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];
            rest = (sum % 11);
            rest = rest < 2 ? 0 : 11 - rest;
            digit = digit + rest.ToString();

            if (!cnpj.EndsWith(digit))
                throw new DomainException($"{cnpj} is not a valid {nameof(cnpj)}");

            return new CNPJ(cnpj);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

    }
}
