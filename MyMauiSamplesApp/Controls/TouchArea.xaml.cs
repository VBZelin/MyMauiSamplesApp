using System.Windows.Input;

namespace MyMauiSamplesApp;

public partial class TouchArea : ContentView
{
    public event EventHandler? Clicked;

    public static readonly BindableProperty IsTouchEnabledProperty =
        BindableProperty.Create(nameof(IsTouchEnabled), typeof(bool), typeof(TouchArea), true);

    public bool IsTouchEnabled
    {
        get => (bool)GetValue(IsTouchEnabledProperty);
        set => SetValue(IsTouchEnabledProperty, value);
    }

    public static readonly BindableProperty IsTouchVisibleProperty =
        BindableProperty.Create(nameof(IsTouchVisible), typeof(bool), typeof(TouchArea), true);

    public bool IsTouchVisible
    {
        get => (bool)GetValue(IsTouchVisibleProperty);
        set => SetValue(IsTouchVisibleProperty, value);
    }

    public static readonly BindableProperty TouchAreaBackgroundViewProperty =
    BindableProperty.Create(
        nameof(TouchAreaBackgroundView),
        typeof(View),
        typeof(TouchArea),
        null,
        propertyChanged: OnTouchAreaBackgroundViewChanged);

    public View? TouchAreaBackgroundView
    {
        get => (View?)GetValue(TouchAreaBackgroundViewProperty);
        set => SetValue(TouchAreaBackgroundViewProperty, value);
    }

    public static readonly BindableProperty TouchAreaOverlayViewProperty =
    BindableProperty.Create(
        nameof(TouchAreaOverlayView),
        typeof(View),
        typeof(TouchArea),
        null,
        propertyChanged: OnTouchAreaOverlayViewChanged);

    public View? TouchAreaOverlayView
    {
        get => (View?)GetValue(TouchAreaOverlayViewProperty);
        set => SetValue(TouchAreaOverlayViewProperty, value);
    }

    public static readonly BindableProperty TouchFeedbackBackgroundColorProperty =
        BindableProperty.Create(nameof(TouchFeedbackBackgroundColor), typeof(Color), typeof(TouchArea), Colors.Transparent);

    public Color TouchFeedbackBackgroundColor
    {
        get => (Color)GetValue(TouchFeedbackBackgroundColorProperty);
        set => SetValue(TouchFeedbackBackgroundColorProperty, value);
    }

    public static readonly BindableProperty TouchFeedbackBorderColorProperty =
        BindableProperty.Create(nameof(TouchFeedbackBorderColor), typeof(Color), typeof(TouchArea), Colors.Transparent);

    public Color TouchFeedbackBorderColor
    {
        get => (Color)GetValue(TouchFeedbackBorderColorProperty);
        set => SetValue(TouchFeedbackBorderColorProperty, value);
    }

    public static readonly BindableProperty TouchFeedbackCornerRadiusProperty =
        BindableProperty.Create(nameof(TouchFeedbackCornerRadius), typeof(int), typeof(TouchArea), 0);

    public int TouchFeedbackCornerRadius
    {
        get => (int)GetValue(TouchFeedbackCornerRadiusProperty);
        set => SetValue(TouchFeedbackCornerRadiusProperty, value);
    }

    public static readonly BindableProperty TouchFeedbackPressedColorProperty =
        BindableProperty.Create(nameof(TouchFeedbackPressedColor), typeof(Color), typeof(TouchArea), Colors.Transparent);

    public Color TouchFeedbackPressedColor
    {
        get => (Color)GetValue(TouchFeedbackPressedColorProperty);
        set => SetValue(TouchFeedbackPressedColorProperty, value);
    }

    public static readonly BindableProperty TouchFeedbackPressedOpacityProperty =
        BindableProperty.Create(nameof(TouchFeedbackPressedOpacity), typeof(double), typeof(TouchArea), 1.0);

    public double TouchFeedbackPressedOpacity
    {
        get => (double)GetValue(TouchFeedbackPressedOpacityProperty);
        set => SetValue(TouchFeedbackPressedOpacityProperty, value);
    }

    public static readonly BindableProperty TouchFeedbackDisabledColorProperty =
        BindableProperty.Create(nameof(TouchFeedbackDisabledColor), typeof(Color), typeof(TouchArea), Colors.Transparent);

    public Color TouchFeedbackDisabledColor
    {
        get => (Color)GetValue(TouchFeedbackDisabledColorProperty);
        set => SetValue(TouchFeedbackDisabledColorProperty, value);
    }

    public static readonly BindableProperty TouchFeedbackDisabledOpacityProperty =
        BindableProperty.Create(nameof(TouchFeedbackDisabledOpacity), typeof(double), typeof(TouchArea), 0.5);

    public double TouchFeedbackDisabledOpacity
    {
        get => (double)GetValue(TouchFeedbackDisabledOpacityProperty);
        set => SetValue(TouchFeedbackDisabledOpacityProperty, value);
    }

    public static readonly BindableProperty ClickCommandProperty =
        BindableProperty.Create(nameof(ClickCommand), typeof(ICommand), typeof(TouchArea), default(ICommand));

    public ICommand? ClickCommand
    {
        get => (ICommand?)GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    public static readonly BindableProperty SemanticDescriptionProperty =
        BindableProperty.Create(nameof(SemanticDescription), typeof(string), typeof(TouchArea), string.Empty);

    public string SemanticDescription
    {
        get => (string)GetValue(SemanticDescriptionProperty);
        set => SetValue(SemanticDescriptionProperty, value);
    }

    public static readonly BindableProperty SemanticHintProperty =
        BindableProperty.Create(nameof(SemanticHint), typeof(string), typeof(TouchArea), string.Empty);

    public string SemanticHint
    {
        get => (string)GetValue(SemanticHintProperty);
        set => SetValue(SemanticHintProperty, value);
    }

    public static readonly BindableProperty TooltipTextProperty =
        BindableProperty.Create(nameof(TooltipText), typeof(string), typeof(TouchArea), string.Empty);

    public string TooltipText
    {
        get => (string)GetValue(TooltipTextProperty);
        set => SetValue(TooltipTextProperty, value);
    }

    public TouchArea()
    {
        InitializeComponent();
        SetDefaultValues();
    }

    private void SetDefaultValues()
    {
        TouchFeedbackPressedOpacity = (double)GetValue(TouchFeedbackPressedOpacityProperty);
        TouchFeedbackDisabledOpacity = (double)GetValue(TouchFeedbackDisabledOpacityProperty);
    }

    private static void OnTouchAreaBackgroundViewChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is TouchArea control)
        {
            UpdateViewContainer(control.touchAreaBackgroundViewContainer, newValue as View);
        }
    }

    private static void OnTouchAreaOverlayViewChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is TouchArea control)
        {
            UpdateViewContainer(control.touchAreaOverlayViewContainer, newValue as View);
        }
    }

    private static void UpdateViewContainer(View? containerView, View? newView)
    {
        if (containerView is not Grid container) return;
        container.Children.Clear();
        if (newView is not null) container.Children.Add(newView);
    }

    private void OnClicked(object? sender, EventArgs e)
    {
        Clicked?.Invoke(this, EventArgs.Empty);
        if (ClickCommand?.CanExecute(null) == true)
            ClickCommand.Execute(null);
    }
}
