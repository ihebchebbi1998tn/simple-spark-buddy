import { useEffect, useRef } from 'react';

interface Node {
  x: number;
  y: number;
  vx: number;
  vy: number;
  size: number;
  opacity: number;
  connections: number[];
}

const ParticleBackground = () => {
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const nodesRef = useRef<Node[]>([]);
  const animationFrameRef = useRef<number | undefined>(undefined);
  const timeRef = useRef(0);

  useEffect(() => {
    const canvas = canvasRef.current;
    if (!canvas) return;

    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    const resizeCanvas = () => {
      canvas.width = window.innerWidth;
      canvas.height = window.innerHeight;
    };

    const createNetwork = () => {
      const nodeCount = Math.min(150, Math.floor((window.innerWidth * window.innerHeight) / 12000));
      nodesRef.current = [];

      // Create nodes
      for (let i = 0; i < nodeCount; i++) {
        nodesRef.current.push({
          x: Math.random() * canvas.width,
          y: Math.random() * canvas.height,
          vx: (Math.random() - 0.5) * 0.2,
          vy: (Math.random() - 0.5) * 0.2,
          size: Math.random() * 2 + 1.5,
          opacity: Math.random() * 0.6 + 0.4,
          connections: [],
        });
      }

      // Create better connections (neural network style)
      nodesRef.current.forEach((node, i) => {
        // Connect to nearby nodes
        nodesRef.current.forEach((otherNode, j) => {
          if (i !== j) {
            const distance = Math.sqrt(
              Math.pow(node.x - otherNode.x, 2) + Math.pow(node.y - otherNode.y, 2)
            );
            
            // Connect nodes that are close enough
            if (distance < 150 && node.connections.length < 6) {
              if (!node.connections.includes(j)) {
                node.connections.push(j);
              }
            }
          }
        });
        
        // Ensure minimum connections
        if (node.connections.length < 2) {
          const remainingNodes = nodesRef.current
            .map((_, index) => index)
            .filter(index => index !== i && !node.connections.includes(index));
          
          for (let k = 0; k < Math.min(3, remainingNodes.length); k++) {
            const randomIndex = remainingNodes[Math.floor(Math.random() * remainingNodes.length)];
            node.connections.push(randomIndex);
            remainingNodes.splice(remainingNodes.indexOf(randomIndex), 1);
          }
        }
      });
    };

    const drawNetwork = () => {
      // Draw connections first
      nodesRef.current.forEach((node, i) => {
        node.connections.forEach(targetIndex => {
          if (targetIndex < nodesRef.current.length) {
            const target = nodesRef.current[targetIndex];
            const distance = Math.sqrt(
              Math.pow(node.x - target.x, 2) + Math.pow(node.y - target.y, 2)
            );
            
            // Draw all connections with varying opacity based on distance and animation
            const baseOpacity = Math.max(0.2, 1 - distance / 250);
            const animatedOpacity = baseOpacity * (0.8 + 0.4 * Math.sin(timeRef.current * 0.001 + i * 0.1));
            
            ctx.strokeStyle = `rgba(0, 86, 184, ${animatedOpacity})`;
            ctx.lineWidth = 1.2;
            ctx.beginPath();
            ctx.moveTo(node.x, node.y);
            ctx.lineTo(target.x, target.y);
            ctx.stroke();
          }
        });
      });

      // Draw nodes
      nodesRef.current.forEach((node, i) => {
        const pulseOpacity = node.opacity * (0.9 + 0.2 * Math.sin(timeRef.current * 0.002 + i * 0.5));
        
        // Outer glow
        ctx.fillStyle = `rgba(0, 86, 184, ${pulseOpacity * 0.4})`;
        ctx.beginPath();
        ctx.arc(node.x, node.y, node.size * 2.5, 0, Math.PI * 2);
        ctx.fill();
        
        // Main node
        ctx.fillStyle = `rgba(0, 86, 184, ${pulseOpacity})`;
        ctx.beginPath();
        ctx.arc(node.x, node.y, node.size, 0, Math.PI * 2);
        ctx.fill();
      });
    };

    const updateNetwork = () => {
      timeRef.current += 16; // Approximate 60fps
      
      nodesRef.current.forEach(node => {
        // Smooth movement
        node.x += node.vx;
        node.y += node.vy;

        // Boundary wrapping for seamless effect
        if (node.x < -50) node.x = canvas.width + 50;
        if (node.x > canvas.width + 50) node.x = -50;
        if (node.y < -50) node.y = canvas.height + 50;
        if (node.y > canvas.height + 50) node.y = -50;

        // Slight random movement variation
        node.vx += (Math.random() - 0.5) * 0.01;
        node.vy += (Math.random() - 0.5) * 0.01;
        
        // Limit speed
        const maxSpeed = 0.4;
        const speed = Math.sqrt(node.vx * node.vx + node.vy * node.vy);
        if (speed > maxSpeed) {
          node.vx = (node.vx / speed) * maxSpeed;
          node.vy = (node.vy / speed) * maxSpeed;
        }
      });
    };

    const animate = () => {
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      
      updateNetwork();
      drawNetwork();
      
      animationFrameRef.current = requestAnimationFrame(animate);
    };

    resizeCanvas();
    createNetwork();
    animate();

    const handleResize = () => {
      resizeCanvas();
      createNetwork();
    };

    window.addEventListener('resize', handleResize);

    return () => {
      window.removeEventListener('resize', handleResize);
      if (animationFrameRef.current) {
        cancelAnimationFrame(animationFrameRef.current);
      }
    };
  }, []);

  return (
    <canvas
      ref={canvasRef}
      className="fixed inset-0 pointer-events-none z-0"
      style={{ opacity: 0.15 }}
    />
  );
};

export default ParticleBackground;
