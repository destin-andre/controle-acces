namespace ControleAcces.Api.Models.Dto
{
    // Ce DTO représente les données envoyées par le module embarqué (RPi/Arduino)
    // au moment d'une tentative d'accès. Il ne correspond à aucune table en base de données,
    // contrairement aux classes du dossier Models.
    public class VerificationAccesRequest
    {
        // Identifiant du badge scanné.
        public int IdBadge { get; set; }

        // Identifiant de la zone où le badge a été scanné (la porte concernée).
        public int IdZone { get; set; }

        // Résultat de la comparaison biométrique, déjà effectuée par le capteur d'empreinte
        // lui-même (le capteur compare en interne et renvoie juste ce booléen au RPi,
        // qui le transmet ici à l'API).
        public bool EmpreinteValidee { get; set; }
    }
}
