using System;
using System.Collections.Generic;
using System.Linq;
using Rditil.Models;

namespace Rditil.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {

            if (!context.Utilisateurs.Any(u => u.Email == "test"))
            {
                context.Utilisateurs.Add(new Utilisateur
                {
                    Nom = "Testeurosorus",
                    Email = "test",
                    PasswordHash = PasswordHelper.HashPassword("Test"),
                    Score = 0,
                    DernierExamen = DateTime.UtcNow
                });
            }

            var existing = new HashSet<string>(
                context.Questions.Select(q => q.Enonce ?? string.Empty));

            var all40 = new List<Question>();
            void Add(string enonce, params (string txt, bool ok)[] reps)
            {
                var q = new Question { Enonce = enonce };
                foreach (var r in reps)
                    q.Reponses.Add(new Reponse { TextReponse = r.txt, EstCorrect = r.ok });
                all40.Add(q);
            }


            // 40 QCM ITIL4 (formulation pédagogique, 1 seule bonne réponse)
            Add("Que signifie ITIL ?",
                ("Information Technology Infrastructure Library", true),
                ("International Telecom Industry Law", false),
                ("Integrated Technology Interface Layer", false),
                ("Internet Tools for Infrastructure Lifecycle", false));

            Add("Dans ITIL 4, un service est avant tout…",
                ("Un moyen de co-créer de la valeur en facilitant des résultats souhaités", true),
                ("Un serveur applicatif déployé en production", false),
                ("Un contrat financier entre deux entités", false),
                ("Un outil de ticketing", false));

            Add("Que désigne le Système de Valeur des Services (SVS) ?",
                ("L’ensemble des composants et activités qui permettent la co-création de valeur", true),
                ("Uniquement le catalogue de services publié aux clients", false),
                ("La CMDB et ses dépendances techniques", false),
                ("Le plan de capacité et les KPI IT", false));

            Add("Lequel fait partie des composants du SVS ?",
                ("Les principes directeurs", true),
                ("Le plan de test d’intégration", false),
                ("La topologie réseau physique", false),
                ("Le manuel d’exploitation des locaux", false));

            Add("Quel énoncé décrit le mieux un principe directeur ITIL ?",
                ("Une recommandation universelle qui guide une organisation dans toutes les situations", true),
                ("Une procédure obligatoire pour les changements urgents", false),
                ("Un rôle spécifique au centre de services", false),
                ("Un gabarit de rapport imposé par la gouvernance", false));

            Add("Parmi ces principes, lequel est **un** principe directeur ITIL 4 ?",
                ("Se focaliser sur la valeur", true),
                ("Toujours automatiser avant de comprendre", false),
                ("Éviter toute itération", false),
                ("Séparer complètement Dev et Ops", false));

            Add("Combien de dimensions de la gestion des services ITIL 4 sont décrites ?",
                ("Quatre", true),
                ("Trois", false),
                ("Cinq", false),
                ("Huit", false));

            Add("Laquelle est **une** des quatre dimensions ITIL ?",
                ("Organisations et personnes", true),
                ("Compatibilité matérielle", false),
                ("Sécurité des bâtiments uniquement", false),
                ("Normes financières IFRS", false));

            Add("L’amélioration continue vise principalement à…",
                ("Aligner, mesurer et améliorer en permanence produits, services et pratiques", true),
                ("Documenter tous les incidents majeurs", false),
                ("Supprimer les CAB", false),
                ("Automatiser les sauvegardes", false));

            Add("Quel est l’objectif de la pratique 'Gestion des incidents' ?",
                ("Restaurer le service le plus rapidement possible", true),
                ("Identifier la cause racine de manière définitive", false),
                ("Valider le plan de capacité annuel", false),
                ("Gérer le budget des licences", false));

            Add("Quel est l’objectif de la pratique 'Gestion des problèmes' ?",
                ("Réduire la probabilité et l’impact des incidents en identifiant les causes", true),
                ("Traiter toutes les demandes d’accès", false),
                ("Gérer la satisfaction client", false),
                ("Administrer la base de connaissances", false));

            Add("La pratique 'Enablement des changements' (Change Enablement) porte sur…",
                ("Maximiser la valeur et réduire les risques des changements", true),
                ("Refuser tous les changements non urgents", false),
                ("Assurer la saisie de temps des équipes", false),
                ("Remplacer la gestion de configuration", false));

            Add("Qu’est-ce qu’une 'Demande de service' (Service Request) ?",
                ("Une requête d’un utilisateur pour quelque chose à fournir (info, accès, standard…)", true),
                ("Un incident majeur", false),
                ("Une alerte de monitoring", false),
                ("Un changement d’urgence", false));

            Add("Le 'Centre de services' (Service Desk) a pour rôle principal de…",
                ("Être le point de contact et de communication avec les utilisateurs", true),
                ("Déployer le code en production", false),
                ("Définir la stratégie IT", false),
                ("Administrer la base de données", false));

            Add("La gestion des niveaux de service (SLM) vise à…",
                ("Établir, négocier, monitorer et améliorer les objectifs de service", true),
                ("Bloquer les demandes hors périmètre", false),
                ("Remplacer les contrats fournisseurs", false),
                ("Écrire les modes opératoires détaillés", false));

            Add("Dans le flux de valeur 'Planifier' (Plan), on cherche surtout à…",
                ("Comprendre la vision, l’orientation et l’état actuel", true),
                ("Analyser les incidents majeurs uniquement", false),
                ("Valider les sauvegardes quotidiennes", false),
                ("Exécuter les changements urgents", false));

            Add("Le flux 'Améliorer' (Improve) démarre idéalement par…",
                ("Une compréhension partagée du problème et de la valeur attendue", true),
                ("L’achat d’un nouvel outil d’abord", false),
                ("La suppression des KPI existants", false),
                ("Un CAB extraordinaire", false));

            Add("Qu’appelle-t-on 'problème connu' ?",
                ("Un problème déjà analysé avec une cause connue et une solution de contournement", true),
                ("Un incident qui revient chaque jour", false),
                ("Une demande planifiée", false),
                ("Un changement standard", false));

            Add("Un 'changement standard' est…",
                ("Un changement pré-approuvé, à faible risque, documenté et fréquent", true),
                ("Un changement non planifié en heures non ouvrées", false),
                ("Tout changement d’urgence", false),
                ("Un changement qui nécessite toujours le CAB", false));

            Add("La pratique 'Gestion des actifs IT' couvre…",
                ("Le cycle de vie et la valeur des actifs pour optimiser coût, risque et utilité", true),
                ("Le paramétrage du pare-feu", false),
                ("Le test de charge applicatif", false),
                ("Uniquement la gestion des contrats RH", false));

            Add("La 'Gestion des configurations de service' maintient…",
                ("Des informations précises et fiables sur les configurations et leurs relations", true),
                ("L’intégralité des logs d’audit pendant 10 ans", false),
                ("Le plan de recrutement", false),
                ("Les manuels d’utilisation des salles", false));

            Add("Quelle affirmation est vraie sur les 'KPI' ?",
                ("Ils doivent mesurer ce qui a de l’importance pour la valeur et les objectifs", true),
                ("Ils doivent être identiques pour tous les services", false),
                ("Ils remplacent les retours utilisateurs", false),
                ("Ils doivent être uniquement techniques", false));

            Add("Un 'SLA' bien conçu doit…",
                ("Être compréhensible, réaliste, mesurable et aligné sur la valeur", true),
                ("Avoir le plus d’indicateurs possibles", false),
                ("Exclure toute pénalité", false),
                ("Être confidentiel pour les équipes", false));

            Add("La gestion des releases concerne…",
                ("La mise à disposition de versions en respectant exigences et contraintes", true),
                ("La supervision temps réel", false),
                ("Le dimensionnement réseau", false),
                ("La gestion des accès utilisateurs", false));

            Add("‘Shift-left’ dans le support signifie…",
                ("Déplacer la résolution au plus tôt/proche de l’utilisateur (self-service, L1)", true),
                ("Reporter les tickets au lendemain", false),
                ("Externaliser le support", false),
                ("Passer tous les tickets au niveau 3", false));

            Add("Une 'base de connaissances' efficace doit être…",
                ("Facile à chercher, maintenue et orientée vers la résolution", true),
                ("Réservée aux experts uniquement", false),
                ("Non versionnée pour aller plus vite", false),
                ("Remplie uniquement après les audits", false));

            Add("La gestion des fournisseurs vise surtout à…",
                ("Obtenir la valeur attendue des fournisseurs via relations et contrats adaptés", true),
                ("Remplacer la DSI", false),
                ("Forcer une seule technologie", false),
                ("Éviter tout SLA externe", false));

            Add("Dans 'Obtenir/Construire' (Obtain/Build), on…",
                ("Élabore ou acquiert les composants nécessaires au service", true),
                ("Ferme les incidents majeurs", false),
                ("Valide les objectifs de niveau de service", false),
                ("Définit la stratégie d’entreprise", false));

            Add("Dans 'Fournir et supporter' (Deliver & Support), on…",
                ("Exécute et supporte les services au quotidien pour les utilisateurs", true),
                ("Rédige la vision stratégique", false),
                ("Choisit les fournisseurs stratégiques", false),
                ("Approuve les portefeuilles projets", false));

            Add("Une 'story utilisateur' est utile surtout pour…",
                ("Décrire le besoin en termes de valeur pour l’utilisateur", true),
                ("Remplacer l’analyse de risques", false),
                ("Écrire un SLA juridique", false),
                ("Éviter la documentation", false));

            Add("Un 'workaround' (solution de contournement) sert à…",
                ("Réduire ou éliminer l’impact d’un incident en attendant une vraie correction", true),
                ("Empêcher tout changement", false),
                ("Supprimer le ticket", false),
                ("Remplacer la cause racine", false));

            Add("Dans ITIL 4, l’automatisation est…",
                ("Utile si elle soutient la valeur et des processus compris et améliorés", true),
                ("Toujours prioritaire sur l’amélioration", false),
                ("Interdite pour le support", false),
                ("Réservée à la production", false));

            Add("Quel est l’intérêt d’un 'problème majeur' (major problem) ?",
                ("Concentrer l’analyse et l’amélioration sur des causes à fort impact", true),
                ("Indiquer un incident P1 en cours", false),
                ("Remplacer les RCA", false),
                ("Décrire un changement urgent", false));

            Add("Un 'Service Catalog' doit…",
                ("Présenter clairement les offres et options accessibles aux clients/utilisateurs", true),
                ("Lister tous les serveurs et leurs IP", false),
                ("Remplacer le CMDB", false),
                ("Contenir uniquement des KPI techniques", false));

            Add("La 'Value Stream Map' sert à…",
                ("Visualiser les étapes/attentes pour optimiser le flux de valeur", true),
                ("Lister les incidents du mois", false),
                ("Décrire l’organigramme", false),
                ("Créer un SLA", false));

            Add("La gestion des disponibilités vise à…",
                ("Assurer que les services atteignent les niveaux de disponibilité convenus", true),
                ("Augmenter la capacité CPU", false),
                ("Réduire les coûts fournisseurs", false),
                ("Supprimer les sauvegardes", false));

            Add("Quel est le meilleur **premier réflexe** avant de créer un nouveau service ?",
                ("Comprendre la valeur attendue et les résultats recherchés", true),
                ("Choisir l’outil ITSM", false),
                ("Rédiger le manuel d’exploitation complet", false),
                ("Acheter de nouveaux serveurs", false));

            Add("Le 'problème' se distingue de l’'incident' car…",
                ("Le problème concerne la cause sous-jacente ; l’incident est l’interruption/altération", true),
                ("Le problème est toujours urgent", false),
                ("L’incident n’affecte jamais les utilisateurs", false),
                ("Le problème n’est pas documenté", false));

            Add("Le 'Post-incident review' a pour but principal de…",
                ("Apprendre et améliorer pour éviter la répétition du même type d’incident", true),
                ("Attribuer des blâmes individuels", false),
                ("Fermer les demandes en attente", false),
                ("Mettre à niveau tout le parc", false));

            Add("‘Start where you are’ signifie…",
                ("Partir de ce qui existe déjà et en tirer parti avant de tout remplacer", true),
                ("Ignorer l’existant", false),
                ("Toujours repartir from scratch", false),
                ("Choisir un nouvel outil d’abord", false));

            Add("‘Progress iteratively with feedback’ encourage…",
                ("Des pas courts, des retours fréquents, et l’ajustement continu", true),
                ("Un grand projet en une seule livraison", false),
                ("Des cycles sans feedback", false),
                ("Uniquement des KPI techniques", false));

            Add("‘Collaborate and promote visibility’ signifie…",
                ("Travailler ensemble et rendre l’information visible pour prendre de meilleures décisions", true),
                ("Limiter la communication aux managers", false),
                ("Masquer les erreurs", false),
                ("Remplacer la gouvernance", false));

            var toAdd = all40.Where(q => !existing.Contains(q.Enonce ?? string.Empty)).ToList();
            if (toAdd.Count > 0)
            {
                context.Questions.AddRange(toAdd);
                context.SaveChanges();
            }
        }
    }
}
