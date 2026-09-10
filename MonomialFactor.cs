namespace LineAproximator
{
    public readonly struct MonomialFactor(char variable, double exponent)
    {
        public readonly char Variable { get; } = variable;
        public readonly double Exponent { get; } = exponent;

        public readonly MonomialFactor Pow(double value) =>
            new(
                Variable,
                Exponent * value
            );

        public static MonomialFactor operator *(MonomialFactor a, MonomialFactor b)
        {
            if (a.Variable != b.Variable)
                throw new ArgumentException($"Cannot multiply factors with different variables: '{a.Variable}' and '{b.Variable}'.");
            return new(a.Variable, a.Exponent + b.Exponent);
        }

        public static MonomialFactor operator /(MonomialFactor a, MonomialFactor b)
        {
            if (a.Variable != b.Variable)
                throw new ArgumentException($"Cannot divide factors with different variables: '{a.Variable}' and '{b.Variable}'.");
            return new(a.Variable, a.Exponent - b.Exponent);
        }

        public override string ToString()
        {
            if (Exponent == 1)
                return Variable.ToString();

            return $"({Variable}^{Exponent})";
        }
    }
}
