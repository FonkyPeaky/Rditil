Liste des classes ajoutées :


Data/

AppDbContext.cs — Contexte EF Core, définit les DbSet<> des entités 

AppDbContextFactory.cs — Factory utilisée pour la création design-time du contexte (migrations)


Migrations/

20250520095124_InitialCreate.cs — Migration initiale des tables de base 

20250520115918_SeedAdmin.cs — Migration qui ajoute un utilisateur admin par défaut 

AppDbContextModelSnapshot.cs — Snapshot EF généré automatiquement pour suivre l'état du modèle 



Models/

Examen.cs — Entité représentant une session d’examen avec score, date, etc.

ExamenQuestion.cs — Table de liaison entre un examen et ses questions

Question.cs — Représente une question posée dans un examen

Reponse.cs — Représente une réponse possible pour une question (en base)

ReponseChoix.cs — Modèle temporaire en mémoire, pour représenter les choix sélectionnés dans l'UI

Utilisateur.cs — Entité représentant un utilisateur (nom, email, rôle, etc.)



Services/

AuthService.cs — Service de connexion (login) vérifiant l'email et le mot de passe dans la base



ViewModels/

AdminViewModel.cs — Gère l'affichage et la logique de la page admin CRUD utilisateurs

ExamViewModel.cs — Gère le tirage des questions, les réponses, le timer et la navigation de l'examen

LoginViewModel.cs — Gère la logique de connexion utilisateur

QuestionViewModel.cs — (Alias ou ancienne version d’ExamViewModel)

ResultViewModel.cs — (prévu pour gérer les scores et affichage final)



Views/

LoginPage.xaml — Interface de connexion

WelcomePage.xaml — Page d'accueil avec les consignes avant de commencer l'examen

QuestionPage.xaml — Affichage d'une question avec 4 réponses et navigation

AdminPanel.xaml — Interface admin CRUD des utilisateurs

EndPage.xaml — (prévu pour afficher le score et message de fin)

