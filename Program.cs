using System.Numerics;

namespace LineAproximator
{
    internal class Program
    {
        static void Main()
        {
            List<Vector2> points = [];

            while (true)
            {
                Console.Write("Digite o X do ponto: ");
                double x = double.Parse(Console.ReadLine()!);

                Console.Write("Digite o Y do ponto: ");
                double y = double.Parse(Console.ReadLine()!);

                points.Add(new((float)x, (float)y));

                Console.WriteLine();
                Console.WriteLine("1 - Adicionar outro ponto");
                Console.WriteLine("2 - Calcular aproximação");
                Console.Write("Escolha: ");

                string? option = Console.ReadLine();

                if (option == "2")
                    break;

                Console.WriteLine();
            }

            Vector2 result = CalculateLineApproximation([.. points]);

            Console.WriteLine();
            Console.WriteLine("Coeficientes:");
            Console.WriteLine($"A = {result.X}");
            Console.WriteLine($"B = {result.Y}");

            Console.ReadLine();
        }

        static Vector2 CalculateLineApproximation(Vector2[] points)
        {
            var errorPolynomial = GetErrorPolynomial(points);

            var derivativeA = Derivate(errorPolynomial, 'A');
            var derivativeB = Derivate(errorPolynomial, 'B');

            var a = SolveLinearEquation(GetEquationA(derivativeA, derivativeB, 'B'), 'A');
            var b = SolveLinearEquation(GetEquationA(derivativeA, derivativeB, 'A'), 'B');

            return new((float)a, (float)b);
        }

        static Polynomial GetEquationA(Polynomial derivativeA, Polynomial derivativeB, char variable)
        {
            var coefficientInDerivativeA = derivativeA.Monomials
                .First(m => m.Factors.Any(f => f.Variable == variable))
                .Coefficient;

            var coefficientInDerivativeB = derivativeB.Monomials
                .First(m => m.Factors.Any(f => f.Variable == variable))
                .Coefficient;

            var ratio = coefficientInDerivativeA / coefficientInDerivativeB;

            return derivativeA - derivativeB * ratio;
        }

        static Polynomial GetErrorPolynomial(Vector2[] points)
        {
            var errorPolynomial = new Polynomial([]);
            foreach (Vector2 point in points)
            {
                Polynomial pointPolynomial = new([
                    new(point.X, [new('A', 1)]),
                    new(1, [new('B', 1)]),
                    new(-point.Y, [new('C', 0)])
                ]);
                errorPolynomial += pointPolynomial.Pow(2); ;
            }

            return errorPolynomial;
        }

        static double SolveLinearEquation(Polynomial equation, char variable)
        {
            var variableMonomial = equation.Monomials.First(
                m => m.Factors.Length == 1 &&
                     m.Factors[0].Variable == variable &&
                     m.Factors[0].Exponent == 1
            );

            var constantMonomial = equation.Monomials.FirstOrDefault(
                m => m.Factors.Length == 0
            );

            if (variableMonomial.Coefficient == 0)
                throw new InvalidOperationException(
                    "Cannot solve an equation with a zero variable coefficient."
                );

            return -constantMonomial.Coefficient / variableMonomial.Coefficient;
        }

        static Polynomial Derivate(Polynomial polynomial, char variable) =>
            new([.. polynomial.Monomials.Select(m => Derivate(m, variable))]);

        static Monomial Derivate(Monomial monomial, char variable)
        {
            var variableFactor = monomial.Factors
                .FirstOrDefault(f => f.Variable == variable);

            if (variableFactor.Exponent == 0)
                return Monomial.Zero;

            var newFactors = monomial.Factors
                .Where(f => f.Variable != variable)
                .ToList();

            newFactors.Add(
                new(variable, variableFactor.Exponent - 1)
            );

            return new(
                monomial.Coefficient * variableFactor.Exponent,
                [.. newFactors]
            );
        }
    }
}
