/**
 * Order Service — encapsulates all order-related data operations.
 * Currently uses mock data. Replace internals with API calls for backend integration.
 */
import type { Order, OrderStats, OrderDetail, RecruiterRole } from '@/types/recruiter';
import { getOrdersByRole as getMockOrders } from '@/data/recruiterMockData';

export const orderService = {
  /** Fetch orders visible to the current user */
  async getOrders(role: RecruiterRole, userName: string): Promise<Order[]> {
    // TODO: Replace with API call
    // return apiClient.get(`/api/recruiter/orders?role=${role}&user=${userName}`, true);
    return getMockOrders(role, userName);
  },

  /** Compute aggregate stats from orders */
  computeStats(orders: Order[]): OrderStats {
    return {
      totalOrders: orders.length,
      activeOrders: orders.filter(o => o.status === 'active').length,
      totalCandidates: orders.reduce((sum, o) => sum + o.candidates, 0),
      totalIntegrated: orders.reduce((sum, o) => sum + o.integrated, 0),
      totalRevenue: orders.reduce((sum, o) => sum + parseInt(o.amount.replace(/\s/g, '').replace('TND', '')), 0),
      avgConversion: orders.length > 0
        ? Math.round(orders.reduce((sum, o) => sum + (o.delivered > 0 ? (o.integrated / o.delivered) * 100 : 0), 0) / orders.length)
        : 0,
    };
  },

  /** Get detailed order info */
  async getOrderDetail(order: Order): Promise<OrderDetail> {
    // TODO: Replace with API call
    return {
      ...order,
      createdAt: order.date + ' 09:30',
      updatedAt: '05/02/2024 14:45',
      paymentStatus: 'paid',
      paymentDate: '16/01/2024',
      paymentMethod: 'Virement bancaire',
      invoiceRef: 'FAC-2024-' + order.id.split('-')[2],
      contact: {
        name: 'Wael Ben Ali',
        email: 'wael.benali@techcall.tn',
        phone: '+216 22 123 456',
      },
      criteria: {
        languages: ['Français', 'Arabe'],
        experience: '1-3 ans',
        activities: [order.activity],
        availability: 'Immédiate',
      },
      leads: [
        { id: 'L-001', name: 'Mohamed Benali', status: 'integre', phone: '+216 22 ***', score: 92 },
        { id: 'L-002', name: 'Fatima Trabelsi', status: 'retenu', phone: '+216 23 ***', score: 88 },
        { id: 'L-003', name: 'Ahmed Sahli', status: 'entretien', phone: '+216 24 ***', score: 85 },
        { id: 'L-004', name: 'Sara Ben Amor', status: 'appele', phone: '+216 25 ***', score: 82 },
        { id: 'L-005', name: 'Karim Jebali', status: 'non_joignable', phone: '+216 26 ***', score: 78 },
        { id: 'L-006', name: 'Leila Mansouri', status: 'a_traiter', phone: '+216 27 ***', score: 75 },
      ],
      timeline: [
        { date: order.date + ' 09:30', action: 'Commande créée', user: 'Wael Ben Ali', type: 'create' },
        { date: order.date + ' 09:35', action: 'Paiement validé', user: 'Système', type: 'payment' },
        { date: order.date + ' 10:00', action: 'Traitement en cours', user: 'Équipe CallMatch', type: 'process' },
        { date: '16/01/2024 14:00', action: '5 leads livrés', user: 'Système', type: 'delivery' },
        { date: '18/01/2024 11:30', action: '5 leads livrés', user: 'Système', type: 'delivery' },
        { date: '20/01/2024 16:00', action: '5 leads livrés', user: 'Système', type: 'delivery' },
      ],
    };
  },

  /** Create a new order */
  async createOrder(_data: Partial<Order>): Promise<Order> {
    // TODO: Replace with API call
    throw new Error('Not implemented — connect to backend');
  },
};
