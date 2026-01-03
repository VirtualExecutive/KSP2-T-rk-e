import UnityPy

def env_load(file_path : str) -> UnityPy.Environment:
    return UnityPy.load(file_path)

def env_save(env : UnityPy.Environment, file_path : str):
    env.save(file_path)