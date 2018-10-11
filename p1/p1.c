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
double arctan(double x, int n);
double arctanOdKonca(double x, int n);
double bladBezwzgledny(double x, double xz);
double bladWzgledny(double x, double xz);

int main() {
	double x = 0.5;
	
	// Opcja 1
	FILE *f = fopen("wyniki-1.csv", "w");
	if (f == NULL)
	{
		printf("Błąd przy otwieraniu pliku!\n");
		exit(1);
	}
	
	
	fprintf(f, "%s,%s,%s,%s,%s,%s\n", "x", "n", "Funkcja wbudowana", "Taylor", "Blad bezwzgledny", "Blad wzgledny");
	for(int n = 1; n <= 10; n++) {
		double wynik = atan(x)*log(x+1);
		double mojWynik = arctan(x,n) * logarytm(x,n);
		fprintf(f,"%0.2lf, %d, %.20lf, %.20lf, %.20lf, %.20lf", x,n, wynik, mojWynik, bladBezwzgledny(wynik,mojWynik), bladWzgledny(wynik,mojWynik));
		fprintf(f,"\n");
	}
	
	fclose(f);
	
	
	// Opcja 2
	FILE *f2 = fopen("wyniki-2.csv", "w");
	if (f == NULL)
	{
		printf("Błąd przy otwieraniu pliku!\n");
		exit(1);
	}
	
	
	fprintf(f, "%s,%s,%s,%s,%s,%s\n", "x", "n", "Funkcja wbudowana", "Taylor", "Blad bezwzgledny", "Blad wzgledny");
	for(int n = 1; n <= 10; n++) {
		double wynik = atan(x)*log(x+1);
		double mojWynik = arctanOdKonca(x,n) * logarytmOdKonca(x,n);
		fprintf(f,"%0.2lf, %d, %.20lf, %.20lf, %.20lf, %.20lf", x,n, wynik, mojWynik, bladBezwzgledny(wynik,mojWynik), bladWzgledny(wynik,mojWynik));
		fprintf(f,"\n");
	}
	
	fclose(f2);

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
	int stala = 0;
	
	for(int i = n-1; i >= 0; i-=2) {
		if(i % 2 == 0)
			stala = i+1;
		else
			stala = i;
		iteracja++;
		zmienna = potega(x,stala)/stala;
		if(iteracja % 2 == 0)
			zmienna = -zmienna;
		wynik+=zmienna;
	}
	if(wynik < 0) 
		wynik = -wynik;
		
	return wynik;
}

double bladBezwzgledny(double x, double xz) {
	return fabs(xz-x);
}

double bladWzgledny(double x, double xz) {
	return bladBezwzgledny(x, xz)/fabs(x);
}
