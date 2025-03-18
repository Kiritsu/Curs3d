# Curs3d

Curs3d is a CurseForge modpack downloader tailored for MultiMC. I created this small project because I prefer not to have Overwolf installed on my computer. Although I have always favored MultiMC, it no longer supports downloading CurseForge modpacks. Therefore, I developed this tool.

Please remember to donate to Minecraft modpack authors to support their work.

## Usage 

Curs3d is a command-line interface (CLI) program and does not include a graphical user interface (GUI).

To output the generated folder directly into your MultiMC instance, you can specify an output path using the argument `-o` or `--output`. This parameter is optional; if not specified, the folder will be generated in the current working directory.

To obtain a CurseForge Modpack URL, navigate to the "Files" section of the Modpack, click the three dots to the right of a modpack version line, and then select "Download file." Finally, right-click on your downloads and choose "Copy download link" to get the direct download URL.

Execute the program by running the following command in a command-line tool. Be sure to replace `<url>`, `<user>`, and `<instance_name>` with the appropriate values.

```
$ Curs3d.exe -s "<url>" -o "C:\Users\<user>\AppData\Local\MultiMC\instances\<instance_name>\.minecraft"
```

# Future

The next step will be to support creating an entire MultiMC instance by specifying its name, the Minecraft version, the mod loader type, and version.

# Disclaimer

This program was created solely for educational purposes. The author is not responsible for any bugs, errors, or issues that may arise from its use. The use of this program is not recommended for any critical or sensitive applications. Users are advised to review the code and understand its functionality before using it.

The author shall not be held liable for any direct, indirect, incidental, special, consequential, or punitive damages arising out of the use or inability to use this program, even if the author has been advised of the possibility of such damages.

By using this program, you acknowledge and agree that you do so at your own risk.

## Important Note:

Using this program to download mods from CurseForge may violate CurseForge's Terms of Use. It is your responsibility to ensure that your use of this program complies with all applicable terms and conditions. The author does not endorse or encourage the use of this program in violation of any third-party terms of service.

This project is not affiliated with, endorsed by, or in any way associated with MultiMC or CurseForge.