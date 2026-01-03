import os
import UnityPy

folder_path = r"C:\Program Files (x86)\Kerbal Space Program 2\KSP2_x64_Data\StreamingAssets\aa\StandaloneWindows64"

# dosya sayısını al
file_count = len(os.listdir(folder_path))
print(file_count, "dosya var")

found_files = []

for i, file in enumerate(os.listdir(folder_path)):
    # live progress bar
    print(f"\r{i}/{file_count}", end="")
    # her bundle dosyası
    if file.endswith(".bundle"):
        # bundle dosyasını yükle
        env = UnityPy.load(os.path.join(folder_path, file))
        # bundle dosyasındaki tüm objeleri al
        for obj in env.objects:
            if obj.type.name == "MonoBehaviour":
                data = obj.read()
                if hasattr(data, "m_Name"):
                    #I2Languages ile başlayan dosyaları al
                    if data.m_Name.startswith("I2Languages"):
                        _ = {
                            "file": file,
                            "name": data.m_Name
                        }
                        found_files.append(_)

print(len(found_files), "dosya bulundu")
for data in sorted(found_files, key=lambda x: x["name"]):
    print(data["file"], data["name"])