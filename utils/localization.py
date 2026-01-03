import UnityPy
import json
import Migrations.lang_files
import os


original_localization_folder = "original_localization"
translated_localization_folder = "translated_localization"


def get_original_localization(env : UnityPy.Environment):
    for obj in env.objects:
        if obj.type.name == "MonoBehaviour":
            data = obj.read()
            if hasattr(data, "m_Name"):
                # Herhangi bir dil dosyası ise
                if data.m_Name.startswith("I2Languages"):
                    # İnitilization
                    file_name = data.m_Name + ".json"
                    source = data.mSource
                    terms = {}
                    # Termleri al
                    for term in source.mTerms:
                        terms[term.Term] = term.Languages[0]
                    # Dosyayı yaz
                    os.makedirs(original_localization_folder, exist_ok=True)
                    with open(f"{original_localization_folder}/{file_name}", "w", encoding="utf-8") as f:
                        json.dump(terms, f, indent=4, ensure_ascii=False)
                    
                    

def update_localization():
    translated_terms = {}
    existing_keys_order = []

    # 1️⃣ Mevcut lang.json oku (sırayı koru)
    with open("lang.json", "r", encoding="utf-8") as f:
        lang_data = json.load(f)
        for entry in lang_data.get("entries", []):
            key = entry["key"]
            translated_terms[key] = entry["value"]
            existing_keys_order.append(key)

    # 2️⃣ Orijinal localization dosyalarını oku
    original_terms = {}
    for localization_name in os.listdir(original_localization_folder):
        path = os.path.join(original_localization_folder, f"{localization_name}")
        with open(path, "r", encoding="utf-8") as f:
            original_terms.update(json.load(f))

    # 3️⃣ Eksik key'leri EN SONA ekle
    for key, value in original_terms.items():
        if key not in translated_terms:
            translated_terms[key] = value  # default İngilizce
            existing_keys_order.append(key)

    # 4️⃣ entries formatına geri çevir (sırayı kullan)
    updated_lang_data = {
        "entries": [
            {"key": key, "value": translated_terms[key]}
            for key in existing_keys_order
        ]
    }

    # 5️⃣ lang.json dosyasını güncelle
    with open("lang.json", "w", encoding="utf-8") as f:
        json.dump(updated_lang_data, f, ensure_ascii=False, indent=2)
