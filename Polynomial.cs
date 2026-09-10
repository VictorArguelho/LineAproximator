namespace LineAproximator
{
    public readonly struct Polynomial
    {
        public readonly Monomial[] Monomials { get; }

        public Polynomial(Monomial[] monomials)
        {
            List<Monomial> finalMonomials = [];

            foreach (var monomial in monomials)
            {
                int index = finalMonomials.FindIndex(
                    m => m.HaveSameLiteralPart(monomial)
                );

                if (index >= 0)
                {
                    double coefficient =
                        finalMonomials[index].Coefficient +
                        monomial.Coefficient;

                    finalMonomials[index] = new Monomial(
                        coefficient,
                        [.. finalMonomials[index].Factors]
                    );
                }
                else
                    finalMonomials.Add(monomial);
            }

            finalMonomials.RemoveAll(m => m.Coefficient == 0);

            Monomials = [.. finalMonomials];
        }

        public readonly Polynomial Pow(double exponent)
        {
            if (exponent < 0 || exponent != Math.Truncate(exponent))
                throw new ArgumentException(
                    "Polynomial exponent must be a non-negative integer."
                );

            int value = (int)exponent;

            Polynomial result = new([
                new Monomial(1, [])
            ]);

            for (int i = 0; i < value; i++)
                result *= this;

            return result;
        }

        public static Polynomial operator +(Polynomial a, Polynomial b) =>
            new([
                .. a.Monomials,
                .. b.Monomials
            ]);

        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            Monomial[] negative = [
                .. b.Monomials.Select(
                    m => m * -1
                )
            ];

            return new([
                .. a.Monomials,
                .. negative
            ]);
        }

        public static Polynomial operator +(Polynomial polynomial, Monomial monomial) =>
            new([
                .. polynomial.Monomials,
                monomial
            ]);

        public static Polynomial operator +(Monomial monomial, Polynomial polynomial) =>
            polynomial + monomial;

        public static Polynomial operator -(Polynomial polynomial, Monomial monomial) =>
            new([
                .. polynomial.Monomials,
                monomial * -1
            ]);

        public static Polynomial operator -(Monomial monomial, Polynomial polynomial)
        {
            Monomial[] negative = [
                .. polynomial.Monomials.Select(
                    m => m * -1
                )
            ];

            return new([
                monomial,
                .. negative
            ]);
        }

        public static Polynomial operator *(Polynomial polynomial, double constant) =>
            new([
                .. polynomial.Monomials.Select(
                    m => m * constant
                )
            ]);

        public static Polynomial operator *(double constant, Polynomial polynomial) =>
            polynomial * constant;

        public static Polynomial operator /(Polynomial polynomial, double constant)
        {
            if (constant == 0)
                throw new DivideByZeroException(
                    "Cannot divide a polynomial by zero."
                );

            return new([
                .. polynomial.Monomials.Select(
                    m => m / constant
                )
            ]);
        }

        public static Polynomial operator *(Polynomial polynomial, Monomial monomial) =>
            new([
                .. polynomial.Monomials.Select(
                    m => m * monomial
                )
            ]);

        public static Polynomial operator *(Monomial monomial, Polynomial polynomial) =>
            polynomial * monomial;

        public static Polynomial operator /(Polynomial polynomial, Monomial monomial) =>
            new([
                .. polynomial.Monomials.Select(
                    m => m / monomial
                )
            ]);

        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            List<Monomial> result = [];

            foreach (var monomialA in a.Monomials)
            {
                foreach (var monomialB in b.Monomials)
                {
                    result.Add(monomialA * monomialB);
                }
            }

            return new([.. result]);
        }

        public static Polynomial operator /(Polynomial a, Polynomial b)
        {
            if (b.Monomials.Length == 0)
                throw new DivideByZeroException(
                    "Cannot divide a polynomial by zero."
                );

            throw new NotSupportedException(
                "Polynomial division is not implemented yet."
            );
        }

        public override string ToString()
        {
            if (Monomials.Length == 0)
                return "0";

            string result = "";

            foreach (var monomial in Monomials)
            {
                if (monomial.Coefficient > 0 && result.Length > 0)
                    result += "+";

                result += monomial;
            }

            return result;
        }
    }
}