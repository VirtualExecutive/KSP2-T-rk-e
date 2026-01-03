import utils.unity
import utils.localization
import os

folder_path = r"C:\Program Files (x86)\Kerbal Space Program 2\KSP2_x64_Data\StreamingAssets\aa\StandaloneWindows64"
default_localization_file = "defaultlocalgroup_assets_all.bundle"
research_localization_file = "research_assets_all.bundle"

# dosya kontrolü
if not os.path.exists(os.path.join(folder_path, default_localization_file)):
    print(f"Default localization file not found: {default_localization_file}")
    exit()
if not os.path.exists(os.path.join(folder_path, research_localization_file)):
    print(f"Research localization file not found: {research_localization_file}")
    exit()

default_localization_env = utils.unity.env_load(os.path.join(folder_path, default_localization_file))
research_localization_env = utils.unity.env_load(os.path.join(folder_path, research_localization_file))

utils.localization.get_original_localization(default_localization_env)
utils.localization.get_original_localization(research_localization_env)
utils.localization.update_localization()
print("Localization updated successfully")