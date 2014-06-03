import os
from sys import argv
from mod_pbxproj import XcodeProject

path = argv[1]
fileToAddPath = argv[2]

print('post_process.py xcode build path --> ' + path)
print('post_process.py third party files path --> ' + fileToAddPath)    
    #Before execute this, you better add a check to see whether your change already exist or not, as if user
    #select *Append* rather than *Replace* in Unity when build, this will save you time and avoid duplicates. 
    
print('Step 1: add system libraries ')
    #if framework is optional, add `weak=True`
project = XcodeProject.Load(path +'/Unity-iPhone.xcodeproj/project.pbxproj')
project.add_file_if_doesnt_exist('System/Library/Frameworks/AdSupport.framework', tree='SDKROOT')
project.add_file_if_doesnt_exist('System/Library/Frameworks/CoreData.framework', tree='SDKROOT')
project.add_file_if_doesnt_exist('usr/lib/libz.dylib', tree='SDKROOT')

print('Step 2: add custom libraries and native code to xcode, exclude any .meta file')
files_in_dir = os.listdir(fileToAddPath)
for f in files_in_dir:    
    if not f.startswith('.'): #exclude .DS_STORE on mac
        print f        
    pathname = os.path.join(fileToAddPath, f)
    fileName, fileExtension = os.path.splitext(pathname)
    if not fileExtension == '.meta': #skip .meta file
        if os.path.isfile(pathname):
            print "is file : " + pathname
            project.add_file_if_doesnt_exist(pathname)
        if os.path.isdir(pathname):
            print "is dir : " + pathname
            project.add_folder(pathname, excludes=["^.*\.meta$"])

print('Step 3: change build setting')
project.add_other_buildsetting('DEBUG_INFORMATION_FORMAT', 'dwarf')

print('Step 4: set -fobjc-arc to plugin file')
project.apply_mods(dict(compiler_flags={'-fobjc-arc':["GoogleAnalyticsPlugin.mm"]}))

print('Step 5: save our change to xcode project file')
if project.modified:
    project.backup()
    project.saveFormat3_2()