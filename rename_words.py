from pathlib import Path

def rename_text(file_name, s_word, new_word):
    try:
        #file name
        file = Path(file_name)
        #Convert both the old and new words into lists
        word_list = s_word.split(",")
        new_word_list = new_word.split(",")
        #Loop through the list and replace them
        for i in range(len(word_list)):
            data = file.read_text()
            data = data.replace(word_list[i], new_word_list[i])
            #Save the new text
            file.write_text(data)
        # return f"{s_word} is succesfully renamed to {new_word}"
    except FileNotFoundError:
        return "file name error"


# x = input("Enter file name: ")

# output = rename_text(x,y,z)
# print(output)

import os

def traverse_directory(root_dir, skip_folder: list[str], old_word, new_word):
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
                print(f"Renaming {old_word} to {new_word} in {file_path}")
                rename_text(file_path, old_word, new_word)

    except FileNotFoundError:
        print(f"Error: Directory '{root_dir}' not found.")
    except Exception as e:
        print(f"An error occurred: {e}")


skip_folder = ["obj", "bin", "Upload", ".git", ".github", ".vs", "Migrations"]
# Example usage:
if __name__ == "__main__":
    y = input("Enter words to be renamed seperated by ',': ")
    z = input("Enter new words for the above accordignly: ")
    root_directory = "."  # Current directory, change as needed.
    traverse_directory(root_directory, skip_folder, y, z)