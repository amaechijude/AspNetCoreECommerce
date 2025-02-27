from pathlib import Path
import os

def rename_text(file_name: str, s_word: str, new_word: str) -> None:
    """
    Replaces a word in a text file with a new word.

    Args:
        file_name (str): The name of the file to rename.
        s_word (str): The word to replace.
        new_word (str): The new word to replace with.
    """
    
    file = Path(file_name) #file name

    #Convert both the old and new words into lists
    word_list = s_word.split(",")
    new_word_list = new_word.split(",")

    #Loop through the list and replace them
    for i in range(len(word_list)):
        data = file.read_text()
        data = data.replace(word_list[i], new_word_list[i])
        #Save the new text
        file.write_text(data)

    return None


def traverse_directory(root_dir, skip_folder: list[str], current_word, new_word):
    """
    Traverses a directory and its subdirectories, printing all files,
    skipping a specified folder.

    Args:
        root_dir (str): The root directory to start traversing.
        skip_folder (str): The name of the folder to skip (default: "code").
    """
    try:
        for root, dirs, files in os.walk(root_dir):
            # Modify dirs in-place to skip the specified folder
            for folder in skip_folder:
                if folder in dirs:
                    dirs.remove(folder)

            for file in files:
                file_path = os.path.join(root, file)
                print(f"Renaming {current_word} to {new_word} in {file_path}")
                rename_text(file_path, current_word, new_word)

    except FileNotFoundError:
        print(f"Error: Directory '{root_dir}' not found.")
    except Exception as e:
        print(f"An error occurred: {e}")


# Usage:
if __name__ == "__main__":
    skip_folder = ["obj", "bin", "Upload", ".git", ".github", ".vs", "Migrations"]

    current_word = input("Enter words to be renamed seperated by ',': ")
    new_word = input("Enter new words for the above accordignly: ")

    root_directory = "."  # Current directory, change as needed.
    traverse_directory(root_directory, skip_folder, current_word, new_word)
    print("Done renaming files.")