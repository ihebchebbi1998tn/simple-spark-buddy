namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20241104073610)]
	public class ChangeExampleInPasswordStrength : Migration
	{
		public override void Up()
		{
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Passwords must be at least 4 characters long."],
				"Language = 'en' AND SortOrder = 0");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Passwörter müssen mindestens 4 Zeichen lang sein."],
				"Language = 'de' AND SortOrder = 0");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Les mots de passe doivent comporter au moins 4 caractères."],
				"Language = 'fr' AND SortOrder = 0");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Las contraseñas deben tener al menos 4 caracteres."],
				"Language = 'es' AND SortOrder = 0");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["A jelszónak legalább 4 karakterből kell állnia."],
				"Language = 'hu' AND SortOrder = 0");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Passwords must contain at least 8 characters, including one uppercase letter and one number."],
				"Language = 'en' AND SortOrder = 1");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Passwörter müssen mindestens 8 Zeichen lang sein, darunter mindestens ein Großbuchstabe und eine Zahl."],
				"Language = 'de' AND SortOrder = 1");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Les mots de passe doivent comporter au moins 8 caractères et inclure des lettres majuscules et des chiffres."],
				"Language = 'fr' AND SortOrder = 1");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Las contraseñas deben tener al menos 8 caracteres e incluir letras mayúsculas y números."],
				"Language = 'es' AND SortOrder = 1");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["A jelszónak legalább 8 karakterből kell állnia és tartalmaznia kell nagybetűket és számokat."],
				"Language = 'hu' AND SortOrder = 1");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Passwords must contain at least 8 characters, including at least one uppercase letter, one lowercase letter, one number, and one special character."],
				"Language = 'en' AND SortOrder = 2");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Passwörter müssen mindestens 8 Zeichen lang sein, darunter mindestens ein Großbuchstabe, ein Kleinbuchstabe, eine Zahl und ein Sonderzeichen."],
				"Language = 'de' AND SortOrder = 2");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Les mots de passe doivent comporter au moins 8 caractères et inclure des majuscules, des minuscules, des chiffres et des caractères spéciaux."],
				"Language = 'fr' AND SortOrder = 2");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Las contraseñas deben tener al menos 8 caracteres e incluir mayúsculas, minúsculas, números y caracteres especiales."],
				"Language = 'es' AND SortOrder = 2");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["A jelszónak legalább 8 karakterből kell állnia és tartalmaznia kell nagybetűket, kisbetűket, számokat és speciális karaktereket."],
				"Language = 'hu' AND SortOrder = 2");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Passwords must contain at least 10 characters, including one uppercase letter, one lowercase letter, one number, and one special character."],
				"Language = 'en' AND SortOrder = 3");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Passwörter müssen mindestens 10 Zeichen lang sein, darunter mindestens ein Großbuchstabe, ein Kleinbuchstabe, eine Zahl und ein Sonderzeichen."],
				"Language = 'de' AND SortOrder = 3");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Les mots de passe doivent comporter au moins 10 caractères et inclure des majuscules, des minuscules, des chiffres et des caractères spéciaux."],
				"Language = 'fr' AND SortOrder = 3");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Las contraseñas deben tener al menos 10 caracteres e incluir mayúsculas, minúsculas, números y caracteres especiales."],
				"Language = 'es' AND SortOrder = 3");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["A jelszónak legalább 10 karakterből kell állnia és tartalmaznia kell nagybetűket, kisbetűket, számokat és speciális karaktereket."],
				"Language = 'hu' AND SortOrder = 3");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Passwords must contain at least 16 characters, with a combination of uppercase and lowercase letters, numbers, and special characters."],
				"Language = 'en' AND SortOrder = 4");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Passwörter müssen mindestens 16 Zeichen lang sein und eine Kombination aus Groß- und Kleinbuchstaben, Zahlen und Sonderzeichen enthalten."],
				"Language = 'de' AND SortOrder = 4");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Les mots de passe doivent comporter au moins 16 caractères et inclure un mélange de majuscules, de minuscules, de chiffres et de caractères spéciaux."],
				"Language = 'fr' AND SortOrder = 4");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["Las contraseñas deben tener al menos 16 caracteres e incluir una combinación de mayúsculas, minúsculas, números y caracteres especiales."],
				"Language = 'es' AND SortOrder = 4");
			Database.Update("LU.PasswordStrength",
				["Name"],
				["A jelszónak legalább 16 karakterből kell állnia és tartalmaznia kell nagybetűk, kisbetűk, számok és speciális karakterek keverékét."],
				"Language = 'hu' AND SortOrder = 4");
		}
	}
}
