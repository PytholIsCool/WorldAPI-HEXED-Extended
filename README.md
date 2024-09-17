# WorldAPI HEXED Extension
<p align="center">
<img src="https://github.com/PytholIsCool/Assets/blob/main/Assets/VRC/Hexed/Hexed%20Banner%20Transparent.png" />
</p>

## An Extension to Hacker_1254's Hexedloader button API for VRChat

This extension comes with:
- Hexed Compatability Fix
- Notorious Compatability Fix
- Button Color Styling Fix
- Localization Of The Toggle Button Icons
- Added QMCGroup Control
- Added QMCToggle Control
- Added QMCSlider Control
- Added QMCSelector Control
- Added QMCFuncButton Control
- Added QMCFuncToggle Control
- Various visual bug fixes

and much more!

## Update V2
- Added State Change stuff for toggles
- Added QMCTitle
- Added QMCFuncButton
- QMCSelector, QMCToggle, and QMCFuncButton controls can now be use in collapsible button groups
- Fixed namespaces
- Started work on the Wing API
- I love PDK

## Update V2.0.1
- Improved button component/object formatting and centering

## Update V2.1.0
- Added support for chaining with the following controls:
> QMCFuncButton

> QMCSelector

> QMCSlider

> QMCTitle

> QMCToggle

- Fixed sliders and added support for setting values on the slider through a config (Thanks Cyconi)
- Other small fixes that aren't all that important
- Started work on a multitoggle system for QMCToggles

## Update V2.2.0
2 Updates in one day???!!???!?!!!
- Added a brand new **AddToggle** method! This is a completely custom control and is ***not native to VRChat***

## Update 2.3.0
THREE UPDATES??!?!?!!?!??!?/1/
anyways, heres the changelog:
- Added the brand new QMCFuncToggle control!
- Changed around the QMCFuncButton and QMCFuncToggle method names for more familiarity
- QMCToggle tooltip argument was moved to come before the listener

## Update 2.3.1
FOUR?!??!?!?!??
- Fixed a really stupid mistake that I made. This api version is safe to download

## Update 2.3.2
five...
- made the function toggle control ovverride the sprites instead of setting them

## Update 2.3.3
six T~T
- fixed the sprite setting stuff (more like tweaked it due to majority vote)

## Update 2.4.0
- Fixed any and all controls involving the QMCFuncButton and QMCFuncToggle controls

### Fixes include:
> AddToggle now works properly

> Listeners in the QMCToggleControls as well as the AddToggle methods no longer overlap

> Icon overrides apply properly

> Other stuff

Thank you voids for your patience and I'm sorry for the trouble

## Update 2.5.0
QMCFunc patches:
- Icon overrides apply properly under all cases
- listeners all work properly under all cases

thorough testing has been done.
This build is confirmed to be safe.

## Update 2.5.1
- removed console references which i was using for debugging (thanks voids for pointing this out)

## Non-Versioned Update
- Readjusted versioning system to be more reasonable
- Added bug(s) from the original WorldAPI

## Update 2.5.2
- Fixed various spelling mistakes
- Added several code optimizations
- Fixed VRCPage headers flashing upon being opened
- Removed QMCSlider Reset Button styling
- Fixed QMCToggle and QMCSlider toggle control tooltips

- Visible progress on the Wing API
> Do not use yet.

> The Wing API is not functional nor should it be used as functional code

> The Wing API is still under development and will be released soon 

  ## Known Bug(s)
- Applying a QMC-Type Control to a collapsible button group will put the control at the above all other controls by default
- Custom Main Menu Tabs highlight the profile tab upon clicking


# Credits
- Cyconi
> Good amount of bug fixes here and there (such as parent reference changes, ect), text placement and centering fixes and PLENTY of requests
- Hacker_1254
> Creator of WorldAPI
- Psychotic
> Requests here and there
- Voids
> Several requests, suggestions and bug reports
- SlyFoox/Salad/HisHaven
> Kept me sane (I love you)

*Examples for how to use the new controls can be found in:*

https://github.com/PytholIsCool/WorldAPI-HEXED-Extension-Examples/blob/main/WorldAPI-HEXED-Extension-Examples/HexedBase/Example.cs


<p align="center">
<img src="https://github.com/PytholIsCool/Assets/blob/main/Assets/VRC/World/WorldClient.png" />
</p>
