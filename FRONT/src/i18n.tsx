import i18next from "i18next";
import { initReactI18next } from "react-i18next";

//Import all translation files
import translationEnglish from "./translations/en/global.json";
import translationSpanish from "./translations/es/global.json";

//---Using translation
 const resources = {
     en: {
         translation: translationEnglish,
     },
     es: {
         translation: translationSpanish,
     }
};

i18next
.use(initReactI18next)
.init({
  resources,
  lng:"es"
});

export default i18next;