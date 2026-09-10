# LineAproximator

Aplicativo de console feito em C# que calcula os coeficientes de uma função do primeiro grau que mais aproxima uma correlação entre diversos pontos no plano cartesiano dados como entrada.

## Funcionamento

- Recebe como entrada do usuário vários pontos no plano cartesiano.
- Gera um polinômio em função de variáveis A e B que calcula a diferença quadrática entre cada ponto dado e seu respectivo valor Y para o X da função hipotética do primeiro grau com coeficientes A e B. 
- Realiza derivadas parciais em função de A e em função de B para o polinômio das diferenças, depois iguala ambas as funções derivadas a 0 e junta ambas em um sistema linear.
- Resolve o sistema linear para encontrar os valores de A e B e depois devolve ao usuário o resultado.
