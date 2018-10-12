/*
 * Aleksander Szewczak
 * Uniwersytet Gdański
 * Indeks: 246749
 * Informatyka, III rok
 * Grupa A02
 * Algorytmy Numeryczne
 * Projekt 1
 * ln(x+1)*arctan(x)
 */


#include <stdio.h>
#include <math.h>
#include <stdlib.h>

// ln(x+1) == x-x^2/2+x^3/3-x^4/4+...
// arctan(x) == x-x^3/3+x^5/5 +...
// x nalezy do (-1,1)

double potega(double podstawa, double wykladnik);
double logarytm(double x, int n);
double arctan(double x, int n);
double logarytmOdKonca(double x, int n);
double logarytmPoprzedni(double x, int n);
double logarytmPoprzedniOdKonca(double x, int n);
double arctan(double x, int n);
double arctanOdKonca(double x, int n);
double arctanPoprzedni(double x, int n);
double arctanPoprzedniOdKonca(double x, int n);
double bladBezwzgledny(double x, double xz);
double bladWzgledny(double x, double xz);

int main() {
	double x = -1;
	int n = 10;
	
	// Opcja 1
	FILE *f = fopen("wyniki-1.csv", "w");
	if (f == NULL)
	{
		printf("Błąd przy otwieraniu pliku!\n");
		exit(1);
	}
	
	
	fprintf(f, "%s,%s,%s,%s,%s,%s\n", "x", "n", "Funkcja wbudowana", "Taylor", "Blad bezwzgledny", "Blad wzgledny");
	for(int i = 1; i <= 200; i++) {
		x+=0.01;
		double wynik = atan(x)*log(x+1);
		double mojWynik = arctan(x,n) * logarytm(x,n);
		fprintf(f,"%0.2lf, %d, %.20lf, %.20lf, %.20lf, %.20lf", x,n, wynik, mojWynik, bladBezwzgledny(wynik,mojWynik), bladWzgledny(wynik,mojWynik));
		fprintf(f,"\n");
	}
	
	fclose(f);
	
	
	// Opcja 2
	FILE *f2 = fopen("wyniki-2.csv", "w");
	if (f2 == NULL)
	{
		printf("Błąd przy otwieraniu pliku!\n");
		exit(1);
	}
	
	// Reset x
	x = -1;
	
	
	fprintf(f2, "%s,%s,%s,%s,%s,%s\n", "x", "n", "Funkcja wbudowana", "Taylor", "Blad bezwzgledny", "Blad wzgledny");
	for(int i = 1; i <= 200; i++) {
		x+=0.01;
		double wynik = atan(x)*log(x+1);
		double mojWynik = arctanOdKonca(x,n) * logarytmOdKonca(x,n);
		fprintf(f2,"%0.2lf, %d, %.20lf, %.20lf, %.20lf, %.20lf", x,n, wynik, mojWynik, bladBezwzgledny(wynik,mojWynik), bladWzgledny(wynik,mojWynik));
		fprintf(f2,"\n");
	}
	
	fclose(f2);
	
	// Opcja 3
	FILE *f3 = fopen("wyniki-3.csv", "w");
	if (f3 == NULL)
	{
		printf("Błąd przy otwieraniu pliku!\n");
		exit(1);
	}
	
	// Reset x
	x = -1;
	
	fprintf(f3, "%s,%s,%s,%s,%s,%s\n", "x", "n", "Funkcja wbudowana", "Taylor", "Blad bezwzgledny", "Blad wzgledny");
	for(int i = 1; i <= 200; i++) {
		x+=0.01;
		double wynik = log(x+1) * atan(x);
		double mojWynik = logarytmPoprzedni(x,n) * arctanPoprzedni(x,n);
		fprintf(f3,"%0.2lf, %d, %.20lf, %.20lf, %.20lf, %.20lf", x,n, wynik, mojWynik, bladBezwzgledny(wynik,mojWynik), bladWzgledny(wynik,mojWynik));
		fprintf(f3,"\n");
	}
	
	fclose(f3);
	
	// Opcja 4
	FILE *f4 = fopen("wyniki-4.csv", "w");
	if (f4 == NULL)
	{
		printf("Błąd przy otwieraniu pliku!\n");
		exit(1);
	}
	
	// Reset x
	x = -1;
	
	fprintf(f4, "%s,%s,%s,%s,%s,%s\n", "x", "n", "Funkcja wbudowana", "Taylor", "Blad bezwzgledny", "Blad wzgledny");
	for(int i = 1; i <= 200; i++) {
		x+=0.01;
		double wynik = atan(x) * log(x+1);
		double mojWynik = arctanPoprzedniOdKonca(x,n) * logarytmPoprzedniOdKonca(x,n);
		fprintf(f4,"%0.2lf, %d, %.20lf, %.20lf, %.20lf, %.20lf", x,n, wynik, mojWynik, bladBezwzgledny(wynik,mojWynik), bladWzgledny(wynik,mojWynik));
		fprintf(f4,"\n");
	}
	
	fclose(f4);

	printf("Wygenerowano do plików\n");
	
	return 0;


}

double potega(double podstawa, double wykladnik) {
	double wynik = 1;
	for(int i = 0; i < wykladnik; i++)
		wynik*=podstawa;
	
	return wynik;
}

double logarytm(double x, int n) {
	double wynik = 0;
	double zmienna = 0;
	for(int i = 1; i <= n; i++) {
		zmienna = potega(x,i)/i;
		if(i % 2 == 0)
			zmienna = -zmienna;
		wynik+=zmienna;	
	}
	return wynik;
}

double logarytmOdKonca(double x, int n) {
	double wynik = 0;
	double zmienna = 0;
	for(int i = n; i >= 1; i--) {
		zmienna = potega(x,i)/i;
		if(i % 2 == 0)
			zmienna = -zmienna;
		wynik+=zmienna;	
	}
	return wynik;
}

double arctan(double x, int n) {
	double wynik = 0;
	double zmienna = 0;
	int iteracja = 0;
	for(int i = 1; i <= n; i+=2) {
		iteracja++;
		zmienna = potega(x,i)/i;
		if(iteracja % 2 == 0)
			zmienna = -zmienna;
		wynik+=zmienna;
	}
	return wynik;
}

double arctanOdKonca(double x, int n) {
	double wynik = 0;
	double zmienna = 0;
	int iteracja = 0;
	for(int i = n-1; i >= 0; i-=2) {
		iteracja++;
		zmienna = potega(x,i)/i;
		if(iteracja % 2 == 0)
			zmienna = -zmienna;
		wynik+=zmienna;
	}
	return wynik;
}

double logarytmPoprzedni(double x, int n) {
	double pierwszy = x;
	double wynik = x;
	for(int i = 2; i <= n; i++) {
		pierwszy = pierwszy * (-1) * (i-1) * x / i;
		wynik += pierwszy;
	}
	return wynik;
	
}

double arctanPoprzedni(double x, int n) {
	double pierwszy = x;
	double wynik = x;
	for(int i = 3; i <= n; i+=2) {
		pierwszy = pierwszy * (-1) * x * x / i * (i-2);
		wynik += pierwszy;
	}
	return wynik;
	
}

double logarytmPoprzedniOdKonca(double x, int n) {
	double elementy[n];
	int licznik = 0;
	double pierwszy = x;
	double wynik = x;
	
	for(int i = 2; i <= n; i++) {
		pierwszy = pierwszy * (-1) * (i-1) * x / i;
		elementy[licznik] = pierwszy;
		licznik++;
	}
	
	for(int i = licznik; i >= 0; i--) {
		wynik += elementy[i];
	}
	
	return wynik;
}

double arctanPoprzedniOdKonca(double x, int n) {
	double elementy[n];
	int licznik = 0;
	double pierwszy = x;
	double wynik = x;
	
	for(int i = 3; i <= n; i+=2) {
		pierwszy = pierwszy * (-1) * x * x / i * (i-2);
		elementy[licznik] = pierwszy;
		licznik++;
	}
	
	for(int i = licznik; i >= 0; i--) {
		wynik += elementy[i];
	}
	
	return wynik;
}

double bladBezwzgledny(double x, double xz) {
	return fabs(xz-x);
}

double bladWzgledny(double x, double xz) {
	return bladBezwzgledny(x, xz)/fabs(x);
}
