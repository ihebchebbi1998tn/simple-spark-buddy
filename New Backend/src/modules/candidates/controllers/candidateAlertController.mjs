/**
 * @file candidateAlertController.mjs
 * @description Controller for managing candidate alerts/notifications
 */

import { ObjectId } from 'mongodb';
import CandidateAlertModel from '../models/CandidateAlert.mjs';
import { successResponse, errorResponse } from '../../../utils/responseHelper.mjs';

/**
 * Get all alerts for authenticated candidate
 */
export const getAlerts = async (req, res) => {
  try {
    const candidateId = req.user.id;
    
    console.log('📬 Fetching alerts for candidate:', candidateId);
    
    // Fetch alerts sorted by creation date (newest first)
    const alerts = await CandidateAlertModel.findByCandidateId(candidateId, 50);
    
    // Count unread alerts
    const unreadCount = alerts.filter(alert => !alert.read).length;
    
    console.log(`✅ Found ${alerts.length} alerts (${unreadCount} unread)`);
    
    return successResponse(res, {
      alerts,
      unreadCount
    }, 200);
    
  } catch (error) {
    console.error('❌ Error fetching alerts:', error);
    return errorResponse(res, 'Erreur lors de la récupération des alertes', 500);
  }
};

/**
 * Mark specific alert as read
 */
export const markAlertAsRead = async (req, res) => {
  try {
    const candidateId = req.user.id;
    const { alertId } = req.params;
    
    console.log('✅ Marking alert as read:', alertId);
    
    const result = await CandidateAlertModel.markAsRead(alertId, candidateId);
    
    if (!result) {
      return errorResponse(res, 'Alerte non trouvée', 404);
    }
    
    return successResponse(res, { alert: result }, 200);
    
  } catch (error) {
    console.error('❌ Error marking alert as read:', error);
    return errorResponse(res, 'Erreur lors de la mise à jour de l\'alerte', 500);
  }
};

/**
 * Mark all alerts as read for authenticated candidate
 */
export const markAllAlertsAsRead = async (req, res) => {
  try {
    const candidateId = req.user.id;
    
    console.log('✅ Marking all alerts as read for candidate:', candidateId);
    
    const result = await CandidateAlertModel.markAllAsRead(candidateId);
    
    console.log(`✅ Marked ${result.modifiedCount} alerts as read`);
    
    return successResponse(res, { 
      markedCount: result.modifiedCount 
    }, 200);
    
  } catch (error) {
    console.error('❌ Error marking all alerts as read:', error);
    return errorResponse(res, 'Erreur lors de la mise à jour des alertes', 500);
  }
};

/**
 * Create a new alert (internal helper - can be used by other controllers)
 */
export const createAlert = async (candidateId, alertData) => {
  try {
    const alert = await CandidateAlertModel.create({
      candidate_id: candidateId,
      type: alertData.type,
      title: alertData.title,
      message: alertData.message,
      action_type: alertData.action_type
    });
    
    console.log('✅ Alert created:', alert.title);
    
    return alert;
  } catch (error) {
    console.error('❌ Error creating alert:', error);
    // Don't throw - alerts are non-critical
    return null;
  }
};

/**
 * Delete old alerts (for cleanup - can be called periodically)
 */
export const deleteOldAlerts = async (req, res) => {
  try {
    const candidateId = req.user.id;
    
    const result = await CandidateAlertModel.deleteOld(candidateId, 30);
    
    console.log(`🗑️ Deleted ${result.deletedCount} old alerts`);
    
    return successResponse(res, { 
      deletedCount: result.deletedCount 
    }, 200);
    
  } catch (error) {
    console.error('❌ Error deleting old alerts:', error);
    return errorResponse(res, 'Erreur lors de la suppression des alertes', 500);
  }
};
