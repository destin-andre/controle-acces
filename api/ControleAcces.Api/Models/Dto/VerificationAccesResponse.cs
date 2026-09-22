namespace ControleAcces.Api.Models.Dto
{
    // Ce DTO représente la réponse renvoyée par l'API après une tentative d'accès.
    // Il indique si l'accès est autorisé, et si non, pour quelles raisons précises.
    public class VerificationAccesResponse
    {
        // True si toutes les conditions sont remplies, false sinon.
        public bool Autorise { get; set; }

        // Liste des raisons du refus. Reste vide si Autorise vaut true.
        // Une liste plutôt qu'un simple message unique, car plusieurs conditions
        // peuvent échouer en même temps (par exemple badge inactif ET hors créneau).
        public List<string> Raisons { get; set; } = new List<string>();
    }
}
