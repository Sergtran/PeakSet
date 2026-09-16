export type ChartPoint = {
  label: string
  value: number
}

const width = 320
const height = 130
const padding = 12

export function ProgressChart({ points }: { points: ChartPoint[] }) {
  const values = points.map((point) => point.value)
  const max = Math.max(...values)
  const min = Math.min(...values)
  const span = max - min || 1
  const step = (width - padding * 2) / (points.length - 1)

  const coords = points.map((point, index) => ({
    ...point,
    x: padding + index * step,
    y: height - padding - ((point.value - min) / span) * (height - padding * 2),
  }))

  const area = `0,${height} ${coords.map((c) => `${c.x},${c.y}`).join(' ')} ${width},${height}`

  return (
    <div className="grid gap-1">
      <svg viewBox={`0 0 ${width} ${height}`} className="w-full" role="img">
        <polygon points={area} className="fill-primary/10" />
        <polyline
          points={coords.map((c) => `${c.x},${c.y}`).join(' ')}
          className="fill-none stroke-primary"
          strokeWidth={2}
          strokeLinejoin="round"
        />
        {coords.map((coord) => (
          <circle key={`${coord.label}-${coord.x}`} cx={coord.x} cy={coord.y} r={3} className="fill-primary" />
        ))}
      </svg>
      <div className="flex justify-between text-xs text-muted-foreground">
        <span>{min}</span>
        <span>{max}</span>
      </div>
    </div>
  )
}
