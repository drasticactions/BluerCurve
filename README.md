# BluerCurve

BluerCurve is a port of [Bluecurve](https://github.com/neeeeow/Bluecurve) to [Avalonia](https://avaloniaui.net/).

## Packages

| Package | Contents |
| --- | --- |
| `BluerCurve` | The theme |
| `BluerCurve.ColorPicker` | Styles for `Avalonia.Controls.ColorPicker` |

## Usage

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:bc="https://github.com/drasticactions/BluerCurve">
  <Application.Styles>
    <bc:BluerCurveTheme Variant="Bluecurve" />
    <!-- optional -->
    <StyleInclude Source="avares://BluerCurve.Theme.ColorPicker/BluerCurve.axaml" />
  </Application.Styles>
</Application>
```

### Color schemes

The themes provided by neeeeow/Bluecurve are represented as `Variant` types. It accepts `Bluecurve`, `Grape`, `Strawberry`, `Slate`, `Lime`, `Tangerine`, `BerriesAndCream` and `Gnome`. It can be changed at runtime, and custom seeds can be supplied through the `Palette` property.

## License

GPL-3.0, The Luxi fonts are distributed under their own license (see `src/BluerCurve/Fonts/LICENSE-Luxi.txt`).
