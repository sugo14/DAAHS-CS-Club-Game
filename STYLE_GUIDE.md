# DAAHS Coding Club Style Guide

This is subject to change as our project continues.

## 1. Script Structure
- In your classes, follow this ordering:
    - Constants & Static Variables.
    - Public Variables.
    - Serialized Fields.
    - Private Variables.
    - Static Methods
    - MonoBehavior Methods (e.g Awake, Start, Update, FixedUpdate, OnDestroy, in roughly lifecycle order).
    - Public Methods.
    - Private Methods.
    - Event Handlers / Coroutines.
- Separate groups of variables and individual functions with lines of whitespace where applicable.

## 2. Naming Conventions
- Classes, Methods, and Properties: `PascalCase`.
- Variables: `camelCase`.
- Constants: `UPPER_SNAKE_CASE`.
- Avoid the `Script` suffix on scripts.
- Files must have the same name as the class contained within them.

## 3. Commenting & Documentation
- // Inline comments should begin with a space and a capitalized first letter
- // Separate ideas by line, instead of with periods
- All classes must have an **XML Comment** (generated by typing `///`).
- Complex functions are recommended to have XML Comments at the code author's discretion.
- Use inline comments where logic is not immediately obvious.

## 4. General Recommendations
- Please don't make spelling mistakes!
- Generally avoid the `private` and `this` keywords.
- Do not include unnecessary whitespace.
- Remove autogenerated comments for Start and Update methods (e.g. `// Start is called before the first frame update`).
- Use serialized fields over public variables wherever possible.
- Use ContextMenus instead of booleans for triggering debugging events.
