using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First
{
	/* Oefening 1a
	 * 
	 * Maak een class Customer, met de volgende properties:
	 * - FirstName
	 * - LastName
	 * - DateOfBirth
	 *         
	 * Je kiest zelf het meest geschikte type voor deze properties.
	 * 
	 * Het programma zal deze class gebruiken om je je naam te vragen
	 * en die dan op het scherm te tonen.
	 */

	/* Oefening 1b
	 * 
	 * Voeg de volgende functies toe aan de class Customer:
	 * - een functie 'Age' die je de leeftijd geeft als integer.
	 * - een functie 'Name' die de volledige naam geeft, waarbij FirstName en LastName
	 *	 gescheiden worden door een spatie.
	 * - een functie 'Print', zonder resultaat, die de volledige naam en de leeftijd
	 *   op het scherm toont via Console.WriteLine().
	 */

	/* Oefening 2
	 * 
	 * Maak een class Book met de volgende properties:
	 * - Title
	 * - Author
	 * - RequiredAge
	 * 
	 * Voorzie ook deze functies:
	 * - 'AllowedToRead', met een Customer als argument. Deze 
	 *   functie laat weten of het boek geschikt is voor de
	 *   leeftijd van de gebruiker.
	 * - 'Print' toont alle informatie over het boek op het 
	 *   scherm.
	 *   
	 */   

	/* Oefening 3
	 * 
	 * Maak een class Transaction met de volgende properties
	 * - Book
	 * - Customer
	 * - LoanDate
	 * 
	 * Voorzie deze functies in de class:
	 * - IsLoanExpired. De uitleentijd is verstreken wanneer
	 *   de gebruiker het langer dan 21 dagen in zijn bezit heeft.
	 *   
	 * - Print. Deze functie toont op het scherm de titel van het 
	 *   boek, de naam van de ontlener en de datum waarop het boek
	 *   uitgeleend werd.
	 */

}
