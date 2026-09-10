namespace LineAproximator
{
    public readonly struct Monomial
    {
        public readonly double Coefficient { get; }
        public readonly MonomialFactor[] Factors { get; }

        public Monomial(double coefficient, MonomialFactor[] factors)
        {
            Coefficient = coefficient;

            List<MonomialFactor> finalFactors = [];
            foreach (var factor in factors)
            {
                if (factor.Exponent == 0)
                    continue;

                int index = finalFactors.FindIndex(f => f.Variable == factor.Variable);

                if (index >= 0)
                    finalFactors[index] = finalFactors[index] * factor;
                else
                    finalFactors.Add(factor);
            }

            Factors = [.. finalFactors.Where(f => f.Exponent != 0)];
        }

        public readonly bool HaveSameLiteralPart(Monomial monomial)
        {
            if (Factors.Length != monomial.Factors.Length)
                return false;

            foreach (var factorA in Factors)
            {
                int index = Array.FindIndex(
                    monomial.Factors,
                    factorB => factorB.Variable == factorA.Variable
                );

                if (index < 0)
                    return false;

                if (factorA.Exponent != monomial.Factors[index].Exponent)
                    return false;
            }

            return true;
        }

        public readonly Monomial Pow(double value) =>
            new(
                Math.Pow(Coefficient, value),
                [.. Factors.Select(f => f.Pow(value))]
            );

        public static Monomial operator *(Monomial monomial, double constant) =>
            new(
                monomial.Coefficient * constant,
                [.. monomial.Factors]
            );

        public static Monomial operator *(double constant, Monomial monomial) =>
            monomial * constant;

        public static Monomial operator /(Monomial monomial, double constant)
        {
            if (constant == 0)
                throw new DivideByZeroException("Cannot divide a monomial by zero.");

            return new(
               monomial.Coefficient / constant,
               [.. monomial.Factors]
            );
        }  

        public static Monomial operator *(Monomial a, Monomial b) =>
            new(
                a.Coefficient * b.Coefficient,
                [.. a.Factors, .. b.Factors]
            );

        public static Monomial operator /(Monomial a, Monomial b)
        {
            if (b.Coefficient == 0)
                throw new DivideByZeroException("Cannot divide a monomial by zero.");

            return a * b.Pow(-1);
        }

        public override string ToString()
        {
            string factors = string.Concat(Factors.Select(f => f.ToString()));

            if (Coefficient == 1 && Factors.Length > 0)
                return factors;

            if (Coefficient == -1 && Factors.Length > 0)
                return $"-{factors}";

            return $"{Coefficient}{factors}";
        }

        public static Monomial Zero => new(0, []);
    }
}
